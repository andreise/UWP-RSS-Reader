using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace RssReader
{
    using Common;
    using Model;

    public sealed class NewsChannelItemPageViewModel
    {

        public RssChannelItem NewsChannelItem { get; }

        public ICommand NewsChannelItemPageGoBackCommand { get; }

        public NewsChannelItemPageViewModel(NewsChannelItemPage owner, RssChannelItem newsChannelItem)
        {
            if ((object)owner == null)
                throw new ArgumentNullException(nameof(owner));

            if ((object)newsChannelItem == null)
                throw new ArgumentNullException(nameof(newsChannelItem));

            Contract.EndContractBlock();

            this.NewsChannelItemPageGoBackCommand = new CommandHandler(() => owner.Frame.GoBack());
            this.NewsChannelItem = newsChannelItem;
        }

    }
}
