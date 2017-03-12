using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using static System.FormattableString;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RssReader
{
    using AppSettings;
    using Model;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsChannelsPage : Page
    {
        public NewsChannelsPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.DataContext = new NewsChannelsPageViewModel(this);

            this.Loaded += this.NewsChannelsPage_Loaded;
        }

        public NewsChannelsPageViewModel ViewModel => (NewsChannelsPageViewModel)this.DataContext;

        public static RssChannel CurrentNewsChannel { get; private set; }

        public int GetSelectedItemIndex() => this.NewsChannelsListView.SelectedIndex;

        private async void ShowMessage(string message, string title)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }

        private async void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
                return;

            if (string.IsNullOrWhiteSpace(AddNewsChannelPage.NewRssUri))
                return;

            string newRssUriString = AddNewsChannelPage.NewRssUri;

            AddNewsChannelPage.NewRssUri = null;

            if (!this.ViewModel.NewsChannels.Any(channel => channel.Uri?.Equals(newRssUriString) ?? false))
            {
                await this.ViewModel.AddNewsChannelAsync(newRssUriString);
            }
        }

        private void ViewModel_NewsChannelLoadingFailed(object sender, NewsChannelLoadingFailedEventArgs e)
        {
            if (e.Handled)
                return;

            e.Handled = true;

            this.ShowMessage(e.Message, "News loading error");
        }

        private void NewsChannelsPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigated += this.Frame_Navigated;
            this.ViewModel.NewsChannelLoadingFailed += this.ViewModel_NewsChannelLoadingFailed;
        }

        private void NewsChannelsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurrentNewsChannel = (RssChannel)e.ClickedItem;
            NewsChannelPage.CurrentNewsChannelItemSetNeeded = false;
            this.Frame.Navigate(typeof(NewsChannelPage));
        }

        private async void DeleteNewsChannel_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.NewsChannels.Count == 0)
                return;

            const string yesId = "YES";
            const string noId = "NO";

            int indexToRemove = this.NewsChannelsListView.SelectedIndex;

            var dialog = new MessageDialog(
                Invariant($"Are you sure to delete news channel '{this.ViewModel.NewsChannels[indexToRemove].Title}'?"),
                "Deleting news channel"
            );

            dialog.Commands.Clear();
            dialog.Commands.Add(new UICommand("Yes", null, yesId));
            dialog.Commands.Add(new UICommand("No", null, noId));

            IUICommand command = await dialog.ShowAsync();

            if (command.Id.Equals(yesId))
            {
                this.ViewModel.NewsChannels.RemoveAt(indexToRemove);

                if (
                    this.ViewModel.NewsChannels.Count > 0 &&
                    this.NewsChannelsListView.SelectedItem is null
                )
                {
                    this.NewsChannelsListView.SelectedItem = this.ViewModel.NewsChannels[0];

                    if (indexToRemove > this.NewsChannelsListView.Items.Count - 1)
                        this.NewsChannelsListView.SelectedIndex = NewsChannelsListView.Items.Count - 1;
                    else
                        this.NewsChannelsListView.SelectedIndex = indexToRemove;
                }
            }
        }

        private void HoldingOrRightTapped(FrameworkElement sender)
        {
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(sender);
            flyoutBase.ShowAt(sender);
        }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if (!(senderElement is null))
                this.HoldingOrRightTapped(senderElement);
        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if (!(senderElement is null))
                this.HoldingOrRightTapped(senderElement);
        }
    }
}
