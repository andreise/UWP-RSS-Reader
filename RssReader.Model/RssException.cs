using System;

namespace RssReader.Model
{
    /// <summary>
    /// RSS Exception
    /// </summary>
    public abstract class RssException : Exception
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public RssException(string message, Exception innerException) : base(message ?? string.Empty, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public RssException(string message) : this(message, null)
        {
        }

    }
}
