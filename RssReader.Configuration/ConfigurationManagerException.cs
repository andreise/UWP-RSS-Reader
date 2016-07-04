using System;

namespace RssReader.Configuration
{
    /// <summary>
    /// Configuration Manager Exception
    /// </summary>
    public abstract class ConfigurationManagerException: Exception
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public ConfigurationManagerException(string message, Exception innerException) : base(message ?? string.Empty, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public ConfigurationManagerException(string message) : this(message, null)
        {
        }

    }
}
