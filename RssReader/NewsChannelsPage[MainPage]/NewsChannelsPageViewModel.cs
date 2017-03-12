using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
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

        private static async Task<RssChannel> LoadChannelAsync(string uri) =>
            await RssManager.LoadChannelAsync(uri, ConfigurationManager.Default.VerifyRssVersion);

        private static async Task<RssChannel> LoadChannelAsync(string uri, Action<Exception> exceptionHandler, bool rethrowException)
        {
            if (exceptionHandler is null && rethrowException)
                return await LoadChannelAsync(uri);

            try
            {
                return await LoadChannelAsync(uri);
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

        private static async Task<IEnumerable<RssChannel>> LoadRssChannelsWithExceptionHandlingAsync(IEnumerable<string> uriCollection)
        {
            List<RssChannel> channels = new List<RssChannel>();

            foreach (string uri in uriCollection)
            {
                string errorMessage = null;
                RssChannel channel = await LoadChannelAsync(
                    uri,
                    e => { errorMessage = e.Message; },
                    rethrowException: false
                );

                if (channel is null)
                    channel = new RssChannel(
                        uri,
                        Invariant($"Error occured during loading the News Channel from '{uri}': {errorMessage}" ),
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

        private static IEnumerable<RssChannel> LoadRssChannelsWithExceptionHandling(IEnumerable<string> uriCollection)
        {
            var task = Task.Run(() => LoadRssChannelsWithExceptionHandlingAsync(uriCollection));
            task.Wait();
            return task.Result;
        }

        private readonly NewsChannelsPage owner;

        public ObservableCollection<RssChannel> NewsChannels { get; }

        public ICommand NavigateAddNewsChannelPageCommand { get; }

        public event EventHandler<NewsChannelLoadingFailedEventArgs> NewsChannelLoadingFailed;

        private void OnNewsChannelLoadingFailed(string message) => this.NewsChannelLoadingFailed?.Invoke(this, new NewsChannelLoadingFailedEventArgs(message));

        public NewsChannelsPageViewModel(NewsChannelsPage owner)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            Contract.EndContractBlock();

            this.owner = owner;
            this.NewsChannels = new ObservableCollection<RssChannel>(LoadRssChannelsWithExceptionHandling(AppSettingsManager.Default.RssUriCollection));
            this.NewsChannels.CollectionChanged += this.NewsChannels_CollectionChanged;
            this.NavigateAddNewsChannelPageCommand = new CommandHandler(() => this.owner.Frame.Navigate(typeof(AddNewsChannelPage)));
        }

        private void NewsChannels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AppSettingsManager.Default.SetRssUriCollection(this.NewsChannels.Select(item => item.Uri?.ToString()));
        }

        public async Task AddNewsChannelAsync(string uri)
        {
            if (uri is null)
                throw new ArgumentNullException(paramName: nameof(uri));

            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException(paramName: nameof(uri), message: "Uri is empty.");

            if (this.NewsChannels.Any(channel => channel.Uri?.Equals(uri) ?? false))
                throw new ArgumentException(paramName: nameof(uri), message: "News Channel with the same URI already loaded.");

            Contract.EndContractBlock();

            RssChannel channelToAdd = await LoadChannelAsync(
                uri,
                e => this.OnNewsChannelLoadingFailed(Invariant($"Error occured during loading the News Channel: {e.Message}")),
                rethrowException: false
            );

            if (!(channelToAdd is null))
                this.NewsChannels.Add(channelToAdd);
        }

    }
}
