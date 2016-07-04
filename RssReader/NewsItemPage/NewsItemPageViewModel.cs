using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RssReader
{
    using Common;
    using Model;

    public sealed class NewsItemPageViewModel
    {

        public RssNewsItem NewsItem { get; }

        public ICommand NewsItemPageGoBackCommand { get; }

        public NewsItemPageViewModel(RssNewsItem newsItem, NewsItemPage newsItemPage)
        {
            if ((object)newsItem == null)
                throw new ArgumentNullException(nameof(newsItem));

            if ((object)newsItemPage == null)
                throw new ArgumentNullException(nameof(newsItemPage));

            Contract.EndContractBlock();

            this.NewsItem = newsItem;
            this.NewsItemPageGoBackCommand = new CommandHandler(() => newsItemPage.Frame.GoBack());
        }

    }
}
