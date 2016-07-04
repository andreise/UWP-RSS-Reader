using System;
using System.Windows.Input;

namespace Common
{
    public class CommandHandler : ICommand
    {

        private readonly Func<object, bool> canExecuteFunc;

        private readonly Action<object> executeAction;

        public CommandHandler(Func<object, bool> canExecuteFunc, Action<object> executeAction)
        {
            this.canExecuteFunc = canExecuteFunc ?? ((object)executeAction == null ? (Func<object, bool>)(parameter => false) : parameter => true);
            this.executeAction = executeAction ?? (parameter => { });
        }

        public CommandHandler(Action<object> executeAction) : this(null, executeAction)
        {
        }

        public CommandHandler(Func<bool> canExecuteFunc, Action executeAction) : this(
            (object)canExecuteFunc == null ? (Func<object, bool>)null : parameter => canExecuteFunc(),
            (object)executeAction == null ? (Action<object>)null : parameter => executeAction()
        )
        {
        }

        public CommandHandler(Action executeAction) : this(null, executeAction)
        {
        }

        protected virtual void OnCanExecuteChanged(EventArgs e) => this.CanExecuteChanged?.Invoke(this, e);

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => this.canExecuteFunc(parameter);

        public void Execute(object parameter) => this.executeAction(parameter);

    }
}
