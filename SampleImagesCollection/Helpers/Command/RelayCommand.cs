using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleMVVMWPF.Helpers.Command
{
    public class RelayCommand : ICommand
    {
        #region Members

        private Action<object> m_execute;
        private Func<object, bool> m_canExecute;

        #endregion // Members

        #region Constructor

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.m_execute = execute;
            this.m_canExecute = canExecute;
        }

        #endregion // Constructor

        #region ICommand

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) 
            => this.m_canExecute == null || this.m_canExecute(parameter);

        public void Execute(object parameter)
            => this.m_execute(parameter);

        #endregion // ICommand
    }
}
