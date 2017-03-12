using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using static System.FormattableString;

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
            => RssManager.LoadChannels(uriCollection, ConfigurationManager.Default.VerifyRssVersion);

        private static RssChannel LoadChannelFromUri(string uri)
            => RssManager.LoadChannel(uri, ConfigurationManager.Default.VerifyRssVersion);

        private static RssChannel LoadChannelFromUri(string uri, Action<Exception> exceptionHandler, bool rethrowException)
        {
            if (exceptionHandler is null && rethrowException)
                return LoadChannelFromUri(uri);

            try
            {
                return LoadChannelFromUri(uri);
            }
            catch (Exception e)
            {
                if (!(exceptionHandler is null))
                    exceptionHandler(e);

                if (rethrowException)
                    throw;

                return null;
            }
        }

        private static IEnumerable<RssChannel> LoadRssChannelsFromUriCollectionWithExceptionHandling(IEnumerable<string> uriCollection)
        {
            List<RssChannel> channels = new List<RssChannel>();

            foreach (string uri in uriCollection)
            {
                string errorMessage = null;
                RssChannel channel = LoadChannelFromUri(
                    uri,
                    e => { errorMessage = e.Message; },
                    rethrowException: false
                );

                if (channel is null)
                    channel = new RssChannel(
                        uri,
                        Invariant($"Error occured during news loading from '{uri}': {errorMessage}" ),
                        null,
                        null,
                        null,
                        null,
                        null
                    );

                channels.Add(channel);
            }

            return channels;
        }


        public ICommand NavigateAddNewsChannelPageCommand { get; }

        private readonly NewsChannelsPage owner;

        public NewsChannelsPageViewModel(NewsChannelsPage owner)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            Contract.EndContractBlock();

            this.owner = owner;
            this.NewsChannels = new ObservableCollection<RssChannel>(LoadRssChannelsFromUriCollectionWithExceptionHandling(AppSettingsManager.Default.RssUriCollection));
            this.NavigateAddNewsChannelPageCommand = new CommandHandler(() => this.owner.Frame.Navigate(typeof(AddNewsChannelPage)));
        }

        public void AddNewsChannel(string uri)
        {
            if (uri is null)
                throw new ArgumentNullException(paramName: nameof(uri));

            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException(paramName: nameof(uri), message: "Uri is empty.");

            if (this.NewsChannels.Any(channel => channel.Uri?.Equals(uri) ?? false))
                throw new ArgumentException(paramName: nameof(uri), message: "News with the same URI already loaded.");

            Contract.EndContractBlock();

            RssChannel channelToAdd = LoadChannelFromUri(
                uri,
                e => this.owner.ShowMessage(Invariant($"Error occured during news loading: {e.Message}"), "News loading error"),
                rethrowException: false
            );
            if (!(channelToAdd is null))
                this.NewsChannels.Add(channelToAdd);
        }

    }
}
