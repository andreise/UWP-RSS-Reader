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

            this.DataContext = new NewsChannelsPageViewModel();
        }

        public NewsChannelsPageViewModel ViewModel => (NewsChannelsPageViewModel)this.DataContext;

        public static RssChannel CurrentNewsChannel { get; private set; }

        private void NewsChannelsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurrentNewsChannel = (RssChannel)e.ClickedItem;
            NewsChannelPage.CurrentNewsChannelItemSetNeeded = false;
            this.Frame.Navigate(typeof(NewsChannelPage));
        }

        private async void DeleteNewsChannel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Are you sure to delete news channel?", "Deleting news channel");
            dialog.Commands.Clear();
            dialog.Commands.Add(new UICommand("Yes", null, "YES"));
            dialog.Commands.Add(new UICommand("No", null, "NO"));
            IUICommand command = await dialog.ShowAsync();
            if (command.Id.Equals("YES"))
            {
                int indexToRemove = this.NewsChannelsListView.SelectedIndex;
                this.ViewModel.NewsChannels.RemoveAt(indexToRemove);
                AppSettingsManager.Default.RssUriCollection.RemoveAt(indexToRemove);
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
            if ((object)senderElement != null)
                this.HoldingOrRightTapped(senderElement);
        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if ((object)senderElement != null)
                this.HoldingOrRightTapped(senderElement);
        }
    }
}
