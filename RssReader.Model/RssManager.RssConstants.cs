namespace RssReader.Model
{
    partial class RssManager
    {
        private static class RssNames
        {

            public const string Root = "rss";

            public const string VersionAttribute = "version";

            public static class Channel
            {

                public const string Root = "channel";

                public const string Title = "title";

                public const string Link = "link";

                public const string Description = "description";

                public static class Image
                {
                    public const string Root = "image";

                    public const string Url = "url";

                    public const string Link = "link";

                    public const string Title = "title";
                }

                public const string ChannelLastBuildDate = "lastBuildDate";

                public static class Item
                {
                    public const string Root = "item";

                    public const string Title = "title";

                    public const string Link = "link";

                    public const string Description = "description";

                    public const string PublishDate = "pubDate";

                    public const string Guid = "guid";
                }

            } // Channel

        } // RssConstants
    }
}
