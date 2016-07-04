using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RssReader.Configuration
{
    /// <summary>
    /// Configuration Manager State
    /// </summary>
    public enum ConfigurationManagerState
    {
        /// <summary>
        /// Configuration not loaded
        /// </summary>
        NotLoaded,
        /// <summary>
        /// Error occured during configuration loading
        /// </summary>
        LoadingFailed,
        /// <summary>
        /// Configuration loaded successfully
        /// </summary>
        Loaded
    }

    /// <summary>
    /// Applicaton Configuration Manager
    /// </summary>
    public class ConfigurationManager
    {

        private static class ConfigParamNames
        {
            public const string Root = "configuration";
            public const string UseDefaultRssUriCollection = "useDefaultRssUriCollection";
            public const string DefaultRssUriCollection = "defaultRssUriCollection";
            public const string DefaultRssUri = "defaultRssUri";
            public const string VerifyRssVersion = "verifyRssVersion";
        }

        private static class ConfigPaths
        {
            public const string AssemblyName = "RssReader.Configuration";
            public const string ConfigName = "app.config.xml";
            public static readonly string ConfigUri = Path.Combine(AssemblyName, ConfigName);
            /// <summary>
            /// Static constructor
            /// </summary>
            /// <remarks>Needs for guaranted static fields initialization</remarks>
            static ConfigPaths() { }
        }

        private static class Converter
        {
            public static string NormalizeString(string s) => s?.Trim() ?? string.Empty;

            public static bool ParseBoolean(string s, bool defaultValue) => string.IsNullOrWhiteSpace(s) ? defaultValue : XmlConvert.ToBoolean(s);
        }

        /// <summary>
        /// Configuration manager state
        /// </summary>
        public ConfigurationManagerState State { get; private set; }

        /// <summary>
        /// Resets configuration manager state to not loaded state
        /// </summary>
        public void Reset() => this.State = ConfigurationManagerState.NotLoaded;

        private void VerifyLoaded()
        {
            if (this.State != ConfigurationManagerState.Loaded)
                throw new InvalidOperationException("Configuration not loaded successfully yet.");
        }

        private bool useDefaultRssUriCollection;

        private IReadOnlyCollection<string> defaultRssUriCollection;

        private bool verifyRssVersion;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="load">Load configuration immediately</param>
        /// <exception cref="ConfigurationManagerLoadingException">Throws if an error occured during application cofiguration loading</exception>
        public ConfigurationManager(bool load)
        {
            if (load)
            {
                this.Load();
            }
        }

        /// <summary>
        /// Loads application configuration
        /// </summary>
        /// <exception cref="ConfigurationManagerLoadingException">Throws if an error occured during application cofiguration loading</exception>
        public void Load()
        {
            this.State = ConfigurationManagerState.NotLoaded;
            try
            {
                XDocument configDoc = XDocument.Load(ConfigPaths.ConfigUri);
                this.useDefaultRssUriCollection = Converter.ParseBoolean(configDoc.Root.Element(ConfigParamNames.UseDefaultRssUriCollection)?.Value, true);
                this.defaultRssUriCollection = new ReadOnlyCollection<string>(configDoc.Root.Descendants(ConfigParamNames.DefaultRssUri).Select(element => element.Value).ToArray());
                this.verifyRssVersion = Converter.ParseBoolean(configDoc.Root.Element(ConfigParamNames.VerifyRssVersion)?.Value, false);
            }
            catch (Exception e)
            {
                this.State = ConfigurationManagerState.LoadingFailed;
                throw new ConfigurationManagerLoadingException("Error occured during application cofiguration loading.", e);
            }
            this.State = ConfigurationManagerState.Loaded;
        }

        /// <summary>
        /// Use Default RSS Uri Collection
        /// </summary>
        /// <remarks>Use default RSS collection if application container contains no RSS collection</remarks>
        /// <exception cref="InvalidOperationException">Throws if configuration not loaded successfully yet</exception>
        public bool UseDefaultRssUriCollection
        {
            get
            {
                this.VerifyLoaded();
                return this.useDefaultRssUriCollection;
            }
        }

        /// <summary>
        /// Default RSS Uri Collection
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if configuration not loaded successfully yet</exception>
        public IReadOnlyCollection<string> DefaultRssUriCollection
        {
            get
            {
                this.VerifyLoaded();
                return this.defaultRssUriCollection;
            }
        }

        /// <summary>
        /// Verify RSS Version
        /// </summary>
        public bool VerifyRssVersion
        {
            get
            {
                this.VerifyLoaded();
                return this.verifyRssVersion;
            }
        }

        private static readonly Lazy<ConfigurationManager> defaultInstance = new Lazy<ConfigurationManager>(() => new ConfigurationManager(load: true));

        /// <summary>
        /// Default instance
        /// </summary>
        public static ConfigurationManager Default => defaultInstance.Value;

        /// <summary>
        /// Static constructor
        /// </summary>
        /// <remarks>Needs for guaranted static fields initialization</remarks>
        static ConfigurationManager()
        {
        }

    }
}
