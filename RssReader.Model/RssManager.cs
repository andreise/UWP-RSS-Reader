using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using static System.FormattableString;

namespace RssReader.Model
{

    /// <summary>
    /// RSS Manager
    /// </summary>
    public static partial class RssManager
    {

        /// <summary>
        /// Supported RSS versions
        /// </summary>
        public static IReadOnlyCollection<Version> SupportedRssVersions { get; } = new ReadOnlyCollection<Version>(
            new Version[]
            {
                new Version(2, 0)
            }
        );

        /// <summary>
        /// Static contructor
        /// </summary>
        /// <remarks>Needs for guaranted static fields initialization</remarks>
        static RssManager()
        {
        }

        /// <summary>
        /// Loads the RSS channels content from Uri
        /// </summary>
        /// <param name="uri">RSS Uri</param>
        /// <param name="verifyRssVersion">Verify RSS version is supported</param>
        /// <returns>RSS channel instance</returns>
        /// <exception cref="ArgumentNullException">Throws if the uri is null</exception>
        /// <exception cref="ArgumentException">Throws if the uri is empty or whitespace</exception>
        /// <exception cref="RssReadingException">Throws if an error occured during RSS reading</exception>
        public static async Task<RssChannel> LoadChannelAsync(string uri, bool verifyRssVersion)
        {
            if (uri is null)
                throw new ArgumentNullException(paramName: nameof(uri));

            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException(paramName: nameof(uri), message: "Uri is empty.");

            Contract.EndContractBlock();

            uri = uri.Trim();

            var tempFile = await Downloader.DownloadFileAsync(new Uri(uri));
            XDocument doc;
            try
            {
                doc = XDocument.Load(tempFile.Path);
            }
            catch (Exception e)
            {
                throw new RssReadingException("Error occured during RSS loading.", e);
            }
            finally
            {
                //await tempFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
#pragma warning disable 4014
                tempFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
#pragma warning restore 4014
            }

            if (doc.Root.Name != RssNames.Root)
                throw new RssReadingException("Input RSS is in an incorrect format.");

            if (verifyRssVersion)
            {
                string inputRssVersion = doc.Root.Attribute(RssNames.VersionAttribute)?.Value.Trim() ?? string.Empty;
                if (
                    inputRssVersion.Length == 0 ||
                    !SupportedRssVersions.Any(version => version.ToString() == inputRssVersion)
                )
                    throw new RssReadingException(Invariant($"Input RSS has an unsupported version ({inputRssVersion})."));
            }

            Func<XElement, RssChannelImage> convertToChannelImage = channelImageElement =>
                channelImageElement is null ? null : new RssChannelImage(
                    channelImageElement.Element(RssNames.Channel.Image.Url)?.Value,
                    channelImageElement.Element(RssNames.Channel.Image.Link)?.Value,
                    channelImageElement.Element(RssNames.Channel.Image.Title)?.Value
                );

            Func<XElement, RssChannelItem> convertToNewsItem = newsItemElement =>
                new RssChannelItem(
                    newsItemElement.Element(RssNames.Channel.Item.Title)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.Link)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.Description)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.PublishDate)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.Guid)?.Value
                );

            Func<XElement, RssChannel> convertToChannel = channelElement =>
                new RssChannel(
                    uri,
                    channelElement.Element(RssNames.Channel.Title)?.Value,
                    channelElement.Element(RssNames.Channel.Link)?.Value,
                    channelElement.Element(RssNames.Channel.Description)?.Value,
                    convertToChannelImage(channelElement.Element(RssNames.Channel.Image.Root)),
                    channelElement.Element(RssNames.Channel.ChannelLastBuildDate)?.Value,
                    channelElement.Elements(RssNames.Channel.Item.Root).Select(convertToNewsItem)
                );

            try
            {
                return convertToChannel(doc.Root.Element(RssNames.Channel.Root));
            }
            catch (Exception e)
            {
                throw new RssReadingException("Error occured during the RSS reading.", e);
            }
        }

    }

}
