using static Common.FormattableString;

namespace RssReader.Model
{
    /// <summary>
    /// RSS News Item
    /// </summary>
    public sealed class RssNewsItem
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
        /// Publish Date
        /// </summary>
        public string PublishDate { get; }

        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="link">Link</param>
        /// <param name="description">Description</param>
        /// <param name="publishDate">Publish Date</param>
        /// <param name="guid">Guid</param>
        public RssNewsItem(
            string title,
            string link,
            string description,
            string publishDate,
            string guid
        )
        {
            this.Title = title ?? string.Empty;
            this.Link = link ?? string.Empty;
            this.Description = description ?? string.Empty;
            this.PublishDate = publishDate ?? string.Empty;
            this.Guid = guid ?? string.Empty;
        }

        public string TitleWithPublishDate => Invariant($"{this.Title} ({this.PublishDate})");

        public override string ToString() => TitleWithPublishDate;

    }
}
