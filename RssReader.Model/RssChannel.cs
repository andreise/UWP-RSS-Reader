using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RssReader.Model
{

    /// <summary>
    /// RSS Channel
    /// </summary>
    public sealed class RssChannel
    {

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Image
        /// </summary>
        public RssChannelImage Image { get; }

        /// <summary>
        /// Last Build Date
        /// </summary>
        public string LastBuildDate { get; }

        /// <summary>
        /// News Items
        /// </summary>
        public IReadOnlyList<RssNewsItem> News { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="link">Link</param>
        /// <param name="description">Description</param>
        /// <param name="image">Image</param>
        /// <param name="lastBuildDate">LastBuildDate</param>
        /// <param name="news">News</param>
        public RssChannel(
            string title,
            string link,
            string description,
            RssChannelImage image,
            string lastBuildDate,
            IList<RssNewsItem> news
        )
        {
            this.Title = title ?? string.Empty;
            this.Link = link ?? string.Empty;
            this.Description = description ?? string.Empty;
            this.Image = image ?? new RssChannelImage();
            this.LastBuildDate = lastBuildDate ?? string.Empty;

            Func<IList<RssNewsItem>> copyNews = () =>
                news?.Where(newsItem => (object)newsItem != null).ToArray() ?? new RssNewsItem[0];

            this.News = new ReadOnlyCollection<RssNewsItem>(copyNews());
        }

    }
}
