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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RssReader
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsChannelItemPage : Page
    {

        public NewsChannelItemPage()
        {
            this.InitializeComponent();

            this.DataContext = new NewsChannelItemPageViewModel(this, NewsChannelPage.CurrentNewsChannelItem);

            //string bigTextRow = new string('$', 1000);
            //var bigTextBuilder = new System.Text.StringBuilder((bigTextRow.Length + Environment.NewLine.Length) * 1000);
            //for (int i = 0; i < 1000; i++)
            //    bigTextBuilder.AppendLine(bigTextRow);

            //this.RssNewsItemWebView.NavigateToString(bigTextBuilder.ToString());

            //this.RssNewsItemWebView.NavigateToString(@"<HTML><HEAD><TITLE>Test Page</TITLE></HEAD><BODY><h3><b>Test</b> <u><i>Page</i></u></h3></BODY></HTML>");

            this.RssNewsChannelItemWebView.NavigateToString(this.ViewModel.NewsChannelItem.Description);
        }

        public NewsChannelItemPageViewModel ViewModel => (NewsChannelItemPageViewModel)this.DataContext;

    }
}
