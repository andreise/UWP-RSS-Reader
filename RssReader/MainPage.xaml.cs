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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.DataContext = new MainPageViewModel();
        }

        public MainPageViewModel ViewModel => (MainPageViewModel)this.DataContext;

        public static RssNewsItem CurrentNewsItem { get; private set; }

        private void RssListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurrentNewsItem = (RssNewsItem)e.ClickedItem;
            this.Frame.Navigate(typeof(NewsItemPage));
        }
    }
}
