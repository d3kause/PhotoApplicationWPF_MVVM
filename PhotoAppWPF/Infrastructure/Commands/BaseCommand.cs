using System;
using System.Windows.Input;

namespace PhotoAppWPF.Infrastructure.Commands
{

    /// <summary>
    /// Base class for all command
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    internal abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public virtual bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);
    }
}
