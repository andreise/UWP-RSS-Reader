using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace RssReader
{
    using AppSettings;
    using Common;
    using Configuration;
    using Model;

    public sealed class NewsChannelPageViewModel
    {

        public IReadOnlyCollection<RssChannel> NewsChannel { get; }

        public ICommand NewsChannelPageGoBackCommand { get; }

        public NewsChannelPageViewModel(NewsChannelPage owner, RssChannel newsChannel)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (newsChannel is null)
                throw new ArgumentNullException(nameof(newsChannel));

            Contract.EndContractBlock();

            this.NewsChannelPageGoBackCommand = new CommandHandler(() => owner.Frame.GoBack());
            this.NewsChannel = new ReadOnlyCollection<RssChannel>(new RssChannel[] { newsChannel });
        }

    }
}
