using System.Windows.Input;

namespace ReolMarkedWPF.Helpers
{
    public class RelayCommand : ICommand
    {
        // Felter
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        // Konstruktør
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        // Sørger for, at UI-elementer som knapper automatisk bliver enabled/disabled baseret på CanExecute-logikken.
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value;}
        }

        // CanExecute-metoden, der tjekker om kommandoen kan udføres.
        // Hvis _canExecute er null, returneres der true, hvilket betyder, at kommandoen altid kan udføres.
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Execute-metoden som der kalder den faktiske handling(AddCustomer fx), når kommandoen bliver udført.
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
