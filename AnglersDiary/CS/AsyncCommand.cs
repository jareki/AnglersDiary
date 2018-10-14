using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnglersDiary.CS
{
    public class AsyncCommand : ICommand
    {
        Func<object,Task> execute;
        Func<object, bool> canexecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncCommand(Func<object,Task> execute, Func<object, bool> canexecute = null)
        {
            this.execute = execute;
            this.canexecute = canexecute;
        }

        public bool CanExecute(object parameter) => canexecute == null || canexecute(parameter);

        public async void Execute(object parameter) => await execute(parameter);
        public Task ExecuteAsync(object parameter)
        {
            return execute.Invoke(parameter);
        }
    }
}
