﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Sharpnado.Tasks;
using WichtelGenerator.WPF.Services;

namespace WichtelGenerator.WPF.Commands
{
    public interface IAsyncCommand<TResult> : ICommand
    {
        TaskMonitor<TResult> Execution { get; }
    }


    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command, Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<object>(
                async (parameter, cancellationToken) =>
                {
                    await command();
                    return null;
                },
                canExecute);
        }

        public static AsyncCommand<TResult> Create<TResult>(
            Func<Task<TResult>> command,
            Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<TResult>((parameter, cancellationToken) => command(), canExecute);
        }

        public static AsyncCommand<TResult> Create<TResult>(
            Func<object, Task<TResult>> command,
            Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<TResult>((parameter, cancellationToken) => command(parameter), canExecute);
        }

        public static AsyncCommand<object> Create(
            Func<object, CancellationToken, Task> command,
            Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<object>(
                async (parameter, token) =>
                {
                    await command(parameter, token);
                    return null;
                },
                canExecute);
        }

        public static AsyncCommand<object> Create(Func<object, Task> command, Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<object>(
                async (parameter, token) =>
                {
                    await command(parameter);
                    return null;
                },
                canExecute);
        }

        public static AsyncCommand<TResult> Create<TResult>(
            Func<object, CancellationToken, Task<TResult>> command,
            Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<TResult>(command, canExecute);
        }
    }

    public class AsyncCommand<TResult> : AsyncCommandBase,
        IAsyncCommand,
        IAsyncCommand<TResult>,
        INotifyPropertyChanged,
        IDisposable
    {
        private readonly CancelAsyncCommand _cancelCommand;
        private readonly Func<object, bool> _canExecuteFunc;
        private readonly Func<object, CancellationToken, Task<TResult>> _command;
        private TaskMonitor<TResult> _execution;

        public AsyncCommand(
            Func<object, CancellationToken, Task<TResult>> command,
            Func<object, bool> canExecute = null)
        {
            _command = command;
            _canExecuteFunc = canExecute;
            _cancelCommand = new CancelAsyncCommand();
        }

        public ICommand CancelCommand => _cancelCommand;

        public override bool CanExecute(object parameter = null)
        {
            var isNotExecuting = Execution == null || Execution.IsCompleted;
            var canExecute = _canExecuteFunc == null || _canExecuteFunc(parameter);
            return isNotExecuting && canExecute;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _cancelCommand.NotifyCommandStarting();
            Execution = TaskMonitor<TResult>.Create(_command(parameter, _cancelCommand.Token));

            RaiseCanExecuteChanged();
            await Execution.TaskCompleted;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        ITaskMonitor IAsyncCommand.Execution => Execution;

        public TaskMonitor<TResult> Execution
        {
            get => _execution;

            private set
            {
                _execution = value;
                OnPropertyChanged();
            }
        }

        public void Dispose()
        {
            _cancelCommand?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand, IDisposable
        {
            private bool _commandExecuting;
            private CancellationTokenSource _cts = new CancellationTokenSource();

            public CancellationToken Token => _cts.Token;

            bool ICommand.CanExecute(object parameter)
            {
                return _commandExecuting && !_cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }

            public event EventHandler CanExecuteChanged;

            public void Dispose()
            {
                _cts?.Dispose();
            }

            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested)
                {
                    return;
                }

                _cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            private void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}