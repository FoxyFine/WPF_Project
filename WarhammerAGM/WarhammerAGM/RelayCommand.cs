using System;
using System.Windows.Input;

namespace WarhammerAGM
{
    /*Этот класс реализует интерфейс ICommand, благодаря чему с помощью подобных команды мы сможем направлять вызовы к ViewModel. 
     * Ключевым здесь является метод Execute(), который получает параметр и выполняет действие, переданное через конструктор команды.*/
    public class RelayCommand : ICommand
    {
        Action<object?> execute;
        Func<object?, bool>? canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
