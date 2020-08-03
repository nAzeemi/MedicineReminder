using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Input;

namespace MedicineReminder.Common
{
    /// <summary>
    /// This class represents a Command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// This class is taken from the article http://msdn.microsoft.com/en-us/magazine/dd419663.aspx 
    /// with only a little modification.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new Command that can always execute.
        /// </summary>
        /// <args name="execute">The execution logic.</args>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new Command.
        /// </summary>
        /// <args name="execute">The execution logic.</args>
        /// <args name="canExecute">The execution status logic.</args>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            Debug.Assert(execute != null, "execute action cannot be null");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion // ICommand Members

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
}
