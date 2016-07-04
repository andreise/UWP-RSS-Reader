using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RssReader
{
    using AppSettings;
    using Configuration;
    using Model;

    public sealed class NewsChannelsPageViewModel
    {
        private static class SettingParamNames
        {
            public static string RssUriList = "rssUriList";
        }

        public ObservableCollection<RssChannel> RssChannels { get; }

        private static IReadOnlyList<RssChannel> LoadRssChannelsFromUriList(IReadOnlyList<string> uriList)
            => RssManager.LoadChannelsFromUriList(uriList, ConfigurationManager.Default.VerifyRssVersion);

        public NewsChannelsPageViewModel()
        {
            this.RssChannels = new ObservableCollection<RssChannel>(LoadRssChannelsFromUriList(AppSettingsManager.Default.RssUriCollection));
        }

    }
}
