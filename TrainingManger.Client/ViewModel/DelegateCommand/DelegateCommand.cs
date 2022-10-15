using System.Windows.Input;
using TrainingManager.Model.LogWriter;

namespace TrainingManager.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public DelegateCommand(Action<object> execute) : this(null, execute) { }
        //public DelegateCommand(Action<object, SelectedItemChangedEventArgs> workoutSelected) { }

        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter)
        {

            try
            {
                if (CanExecute(parameter))
                    _execute(parameter);

                //if (!CanExecute(parameter))
                //throw new InvalidOperationException("Command execution is disabled.");
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error(ex.Message);
                throw;
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
