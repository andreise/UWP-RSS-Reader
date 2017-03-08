using System;
using System.Windows.Input;

namespace Common
{

    public class CommandHandler : ICommand
    {

        private static Func<object, bool> ConvertCanExecute(Func<bool> canExecute) =>
            (object)canExecute == null ? null : new Func<object, bool>(parameter => canExecute());

        private static Action<object> ConvertExecute(Action execute) =>
            (object)execute == null ? null : new Action<object>(parameter => execute());

        private readonly Func<object, bool> canExecute;

        private readonly Action<object> execute;

        public CommandHandler(Func<object, bool> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public CommandHandler(Action<object> execute) : this(null, execute)
        {
        }

        public CommandHandler(Func<bool> canExecute, Action execute) : this(ConvertCanExecute(canExecute), ConvertExecute(execute))
        {
        }

        public CommandHandler(Action execute) : this(null, execute)
        {
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? (object)this.execute != null;

        public void Execute(object parameter) => this.execute?.Invoke(parameter);

    }

}
