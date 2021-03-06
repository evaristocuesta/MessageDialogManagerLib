﻿using System;
using System.Windows.Input;

namespace MessageDialogManagerLib
{
    class Command : ICommand
    {
        private readonly Action _execute;

        private readonly Func<bool> _canExecute;

        public Command(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");

            if (canExecute != null)
            {
                _canExecute = canExecute;
            }
        }

        #region ICommand Members  

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke();
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter) && _execute != null)
            {
                _execute.Invoke();
            }
        }
        #endregion
    }
}
