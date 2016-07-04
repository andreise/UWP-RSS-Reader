using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Networking;

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
        public static IReadOnlyList<Version> SupportedRssVersions { get; } = new ReadOnlyCollection<Version>(
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
        /// <returns>RSS channel instance list</returns>
        /// <exception cref="ArgumentNullException">Throws if the uri is null</exception>
        /// <exception cref="RssReadingException">Throws if an error occured during RSS reading</exception>
        public static RssChannel LoadChannelFromUri(string uri, bool verifyRssVersion = false)
        {
            if ((object)uri == null)
                throw new ArgumentNullException(nameof(uri));

            Contract.EndContractBlock();

            IStorageFile rssTempFile = Downloader.DownloadFileFromUri(new Uri(uri));
            XDocument doc;
            try
            {
                try
                {
                    doc = XDocument.Load(rssTempFile.Path);
                }
                catch (Exception e)
                {
                    throw new RssReadingException("Error occured during RSS loading.", e);
                }
            }
            finally
            {
                Task.Run(() => rssTempFile.DeleteAsync(StorageDeleteOption.PermanentDelete));
            }

            if (doc.Root.Name != RssNames.Root)
                throw new RssReadingException("Input RSS is in an incorrect format.");

            if (verifyRssVersion)
            {
                if (!(
                    SupportedRssVersions.Any(version => version.ToString() == doc.Root.Attribute(RssNames.VersionAttribute)?.Value.Trim())
                ))
                    throw new RssReadingException("Input RSS has an unsupported version.");
            }

            Func<XElement, RssChannelImage> convertToChannelImage = channelImageElement =>
                (object)channelImageElement == null ? null : new RssChannelImage(
                    channelImageElement.Element(RssNames.Channel.Image.Url)?.Value,
                    channelImageElement.Element(RssNames.Channel.Image.Link)?.Value,
                    channelImageElement.Element(RssNames.Channel.Image.Title)?.Value
                );

            Func<XElement, RssNewsItem> convertToNewsItem = newsItemElement =>
                new RssNewsItem(
                    newsItemElement.Element(RssNames.Channel.Item.Title)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.Link)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.Description)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.PublishDate)?.Value,
                    newsItemElement.Element(RssNames.Channel.Item.Guid)?.Value
                );

            Func<XElement, RssChannel> convertToChannel = channelElement =>
                new RssChannel(
                    channelElement.Element(RssNames.Channel.Title)?.Value,
                    channelElement.Element(RssNames.Channel.Link)?.Value,
                    channelElement.Element(RssNames.Channel.Description)?.Value,
                    convertToChannelImage(channelElement.Element(RssNames.Channel.Image.Root)),
                    channelElement.Element(RssNames.Channel.ChannelLastBuildDate)?.Value,
                    channelElement.Elements(RssNames.Channel.Item.Root).Select(convertToNewsItem).ToArray()
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

        /// <summary>
        /// Loads the RSS channels content from Uri list
        /// </summary>
        /// <param name="uriList">RSS Uri list</param>
        /// <param name="verifyRssVersion">Verify RSS version is supported</param>
        /// <returns>RSS channel instance list</returns>
        /// <exception cref="ArgumentNullException">Throws if the uri list is null</exception>
        /// <exception cref="RssReadingException">Throws if an error occured during RSS reading</exception>
        public static IReadOnlyList<RssChannel> LoadChannelsFromUriList(IReadOnlyList<string> uriList, bool verifyRssVersion = false)
        {
            if ((object)uriList == null)
                throw new ArgumentNullException(nameof(uriList));

            Contract.EndContractBlock();

            return uriList.Where(uri => (object)uri != null).Select(uri => LoadChannelFromUri(uri, verifyRssVersion)).ToArray();
        }

    }
}
