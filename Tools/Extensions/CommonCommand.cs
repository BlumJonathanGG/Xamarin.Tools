﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tools.Extensions
{
    public class CommonCommand : ICommand
    {
        private bool _CanExecute;
        public bool CanExecute
        {
            get
            {
                bool can;
                if (this.CanExecuteFunction is null)
                {
                    can = true;
                }
                can = CanExecuteFunction.Invoke();
                if (can != _CanExecute)
                {
                    _CanExecute = can;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
                return _CanExecute;
            }
        }
        public event EventHandler CanExecuteChanged;
        private Func<bool> CanExecuteFunction;
        private Action<object> ExecuteAction;
        public CommonCommand(Func<bool> CanExecuteFunction, Action<object> ExecuteAction)
        {
            this.CanExecuteFunction = CanExecuteFunction;
            this.ExecuteAction = ExecuteAction;
        }
        public CommonCommand(Action<object> ExecuteAction)
        {
            this.CanExecuteFunction = null;
            this.ExecuteAction = ExecuteAction;
        }

        public void Execute(object parameter)
        {
            ExecuteAction.Invoke(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute;
        }
    }
}