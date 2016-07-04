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

        public ICommand NewsChannelPageGoBackCommand;

        public NewsChannelPageViewModel(NewsChannelPage owner, RssChannel newsChannel)
        {
            if ((object)owner == null)
                throw new ArgumentNullException(nameof(owner));

            if ((object)newsChannel == null)
                throw new ArgumentNullException(nameof(newsChannel));

            Contract.EndContractBlock();

            this.NewsChannelPageGoBackCommand = new CommandHandler(() => owner.Frame.GoBack());
            this.NewsChannel = new ReadOnlyCollection<RssChannel>(new RssChannel[] { newsChannel });
        }

    }
}
