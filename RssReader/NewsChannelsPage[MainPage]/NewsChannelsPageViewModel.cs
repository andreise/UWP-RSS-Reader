using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace RssReader
{
    using AppSettings;
    using Common;
    using Configuration;
    using Model;

    public sealed class NewsChannelsPageViewModel
    {

        public ObservableCollection<RssChannel> NewsChannels { get; }

        private static IEnumerable<RssChannel> LoadRssChannelsFromUriCollection(IEnumerable<string> uriCollection)
            => RssManager.LoadChannelsFromUriCollection(uriCollection, ConfigurationManager.Default.VerifyRssVersion);

        public NewsChannelsPageViewModel()
        {
            this.NewsChannels = new ObservableCollection<RssChannel>(LoadRssChannelsFromUriCollection(AppSettingsManager.Default.RssUriCollection));
        }

    }
}
