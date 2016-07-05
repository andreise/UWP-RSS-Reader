using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RssReader.AppSettings
{
    using Configuration;

    public sealed class AppSettingsManager
    {
        private static class SettingParamNames
        {
            public static string RssUriCollection = "rssUriCollection";
        }

        private const string uriSeparator = "\r\n";

        private static IReadOnlyCollection<string> ParseUriCollection(string uriCollection) =>
            uriCollection?.Split(
                new string[] { uriSeparator },
                StringSplitOptions.RemoveEmptyEntries
            ).Where(
                uri => !string.IsNullOrWhiteSpace(uri)
            ).Select(
                uri => uri.Trim()
            ).ToArray() ?? new string[0];

        private static string JoinUriCollection(IEnumerable<string> uriCollection) =>
             string.Join(
                 uriSeparator,
                 uriCollection?.Where(
                     uri => !string.IsNullOrWhiteSpace(uri)
                 ).Select(
                     uri => uri.Trim()
                 ) ?? new string[0]
             );

        private readonly ConfigurationManager configManager;

        private readonly ApplicationDataContainer settingsContainer;

        private IEnumerable<string> LoadRssUriCollection()
        {
            var rssUriCollection = ParseUriCollection((string)this.settingsContainer.Values[SettingParamNames.RssUriCollection]);

            if (rssUriCollection.Count == 0)
            {
                if (this.configManager.UseDefaultRssUriCollection)
                {
                    rssUriCollection = this.configManager.DefaultRssUriCollection;
                    this.settingsContainer.Values[SettingParamNames.RssUriCollection] = JoinUriCollection(rssUriCollection);
                }
            }

            return rssUriCollection;
        }

        public ObservableCollection<string> RssUriCollection { get; }

        public AppSettingsManager()
        {
            this.configManager = ConfigurationManager.Default;
            this.settingsContainer = ApplicationData.Current.LocalSettings;
            this.RssUriCollection = new ObservableCollection<string>(this.LoadRssUriCollection());
            this.RssUriCollection.CollectionChanged += this.RssUriCollection_CollectionChanged;
        }

        private void RssUriCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((object)e.NewItems != null && e.NewItems.Cast<string>().Any(item => string.IsNullOrWhiteSpace(item)))
            {
                throw new InvalidOperationException("Empty uri not allowed.");
            }

            this.settingsContainer.Values[SettingParamNames.RssUriCollection] = JoinUriCollection(this.RssUriCollection);
        }

        private static readonly Lazy<AppSettingsManager> defaultInstance = new Lazy<AppSettingsManager>(() => new AppSettingsManager());

        /// <summary>
        /// Default instance
        /// </summary>
        public static AppSettingsManager Default => defaultInstance.Value;

        /// <summary>
        /// Static constructor
        /// </summary>
        /// <remarks>Needs for guaranted static fields initialization</remarks>
        static AppSettingsManager()
        {
        }

    }
}
