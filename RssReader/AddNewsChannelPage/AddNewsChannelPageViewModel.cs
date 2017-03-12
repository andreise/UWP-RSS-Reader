using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace RssReader
{
    using Common;

    public sealed class AddNewsChannelPageViewModel
    {

        public ICommand OKCommand { get; }

        public ICommand CancelCommand { get; }

        public AddNewsChannelPageViewModel(AddNewsChannelPage owner)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            Contract.EndContractBlock();

            this.OKCommand = new CommandHandler(
                () =>
                {
                    AddNewsChannelPage.NewRssUri = owner.RssUri;
                    owner.Frame.GoBack();
                }
            );

            this.CancelCommand = new CommandHandler(
                () =>
                {
                    AddNewsChannelPage.NewRssUri = null;
                    owner.Frame.GoBack();
                }
            );
        }

    }
}
