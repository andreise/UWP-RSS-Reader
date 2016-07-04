using System;

namespace RssReader.Model
{
    /// <summary>
    /// RSS Reading Exception
    /// </summary>
    public class RssReadingException : RssException
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public RssReadingException(string message, Exception innerException) : base(message ?? string.Empty, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public RssReadingException(string message) : this(message, null)
        {
        }

    }
}
