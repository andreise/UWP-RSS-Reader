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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RssReader
{
    using Model;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsChannelPage : Page
    {
        public NewsChannelPage()
        {
            this.InitializeComponent();

            this.DataContext = new NewsChannelPageViewModel(this, NewsChannelsPage.CurrentNewsChannel);

            this.Loaded += NewsChannelPage_Loaded;
        }

        private void NewsChannelPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentNewsChannelItemSetNeeded)
                this.NewsChannelListView.SelectedItem = CurrentNewsChannelItem;
        }

        public NewsChannelPageViewModel ViewModel => (NewsChannelPageViewModel)this.DataContext;

        public static bool CurrentNewsChannelItemSetNeeded { get; set; }

        public static RssChannelItem CurrentNewsChannelItem { get; set; }

        private void NewsChannelListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurrentNewsChannelItem = (RssChannelItem)e.ClickedItem;
            this.Frame.Navigate(typeof(NewsChannelItemPage));
        }
    }
}
