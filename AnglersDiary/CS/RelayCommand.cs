using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnglersDiary
{
    public class RelayCommand : ICommand
    {
        Action<object> execute;
        Func<object, bool> canexecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object,bool> canexecute=null)
        {
            this.execute = execute;
            this.canexecute = canexecute;
        }

        public bool CanExecute(object parameter) => canexecute == null || canexecute(parameter);

        public void Execute(object parameter) => execute(parameter);
    }
}
