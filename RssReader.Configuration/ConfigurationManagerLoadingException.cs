using System;

namespace RssReader.Configuration
{
    /// <summary>
    /// Configuration Manager Loading Exception
    /// </summary>
    public class ConfigurationManagerLoadingException: ConfigurationManagerException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public ConfigurationManagerLoadingException(string message, Exception innerException) : base(message ?? string.Empty, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public ConfigurationManagerLoadingException(string message) : this(message, null)
        {
        }
    }
}
