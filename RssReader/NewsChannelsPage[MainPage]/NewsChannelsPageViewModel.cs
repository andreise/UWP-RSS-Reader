using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace RssReader
{
    using AppSettings;
    using Common;
    using Configuration;
    using Model;
    using static Common.FormattableString;

    public sealed class NewsChannelsPageViewModel
    {

        public ObservableCollection<RssChannel> NewsChannels { get; }

        private static IEnumerable<RssChannel> LoadRssChannelsFromUriCollection(IEnumerable<string> uriCollection)
            => RssManager.LoadChannelsFromUriCollection(uriCollection, ConfigurationManager.Default.VerifyRssVersion);

        private static RssChannel LoadChannelFromUri(string uri)
            => RssManager.LoadChannelFromUri(uri, ConfigurationManager.Default.VerifyRssVersion);

        public ICommand NavigateAddNewsChannelPageCommand { get; }

        private readonly NewsChannelsPage owner;

        public NewsChannelsPageViewModel(NewsChannelsPage owner)
        {
            if ((object)owner == null)
                throw new ArgumentNullException(nameof(owner));

            Contract.EndContractBlock();

            this.owner = owner;
            this.NewsChannels = new ObservableCollection<RssChannel>(LoadRssChannelsFromUriCollection(AppSettingsManager.Default.RssUriCollection));
            this.NavigateAddNewsChannelPageCommand = new CommandHandler(() => this.owner.Frame.Navigate(typeof(AddNewsChannelPage)));
        }

        public void AddNewsChannel(string uri)
        {
            if ((object)uri == null)
                throw new ArgumentNullException(paramName: nameof(uri));

            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException(paramName: nameof(uri), message: "Uri is empty.");

            if (this.NewsChannels.Any(channel => new Uri(channel.Uri).Equals(uri)))
                throw new ArgumentException(paramName: nameof(uri), message: "News channel with the same URI already loaded.");

            Contract.EndContractBlock();

            RssChannel channelToAdd = null;
            try
            {
                channelToAdd = LoadChannelFromUri(uri);
            }
            catch (Exception e)
            {
                owner.ShowMessage(
                    Invariant($"Error occured during news loading: {e.Message}."),
                    "News loading error"
                );
            }
            if ((object)channelToAdd != null)
            {
                this.NewsChannels.Add(channelToAdd);
            }
        }

    }
}
