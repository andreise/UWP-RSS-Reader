namespace RssReader
{

    public sealed class NewsChannelLoadingFailedEventArgs
    {
        public string Message { get; }

        public bool Handled { get; set; }

        public NewsChannelLoadingFailedEventArgs(string message)
        {
            this.Message = message ?? string.Empty;
        }
    }

}
