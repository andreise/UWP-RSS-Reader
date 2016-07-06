namespace RssReader.Model
{

    /// <summary>
    /// RSS Channel Image
    /// </summary>
    public sealed class RssChannelImage
    {

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="link">Link</param>
        /// <param name="title">Title</param>
        public RssChannelImage(
            string url,
            string link,
            string title
        )
        {
            this.Url = url ?? string.Empty;
            this.Link = link ?? string.Empty;
            this.Title = title ?? string.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RssChannelImage(): this(null, null, null)
        {
        }

    }
}
