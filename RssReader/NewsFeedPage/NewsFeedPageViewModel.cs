using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RssReader
{
    using AppSettings;
    using Configuration;
    using Model;

    public sealed class NewsFeedPageViewModel
    {

        public ObservableCollection<RssChannel> RssChannels { get; }

        private static IEnumerable<RssChannel> LoadRssChannelsFromUriCollection(IEnumerable<string> uriCollection)
            => RssManager.LoadChannelsFromUriCollection(uriCollection, ConfigurationManager.Default.VerifyRssVersion);

        public NewsFeedPageViewModel()
        {
            this.RssChannels = new ObservableCollection<RssChannel>(LoadRssChannelsFromUriCollection(AppSettingsManager.Default.RssUriCollection));
        }

    }
}
