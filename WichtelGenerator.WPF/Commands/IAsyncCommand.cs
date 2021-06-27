using System.Threading.Tasks;
using System.Windows.Input;
using Sharpnado.Tasks;

namespace WichtelGenerator.WPF.Commands
{
    public interface IAsyncCommand : ICommand
    {
        //Braucht Nuget: Sharpnado.TaskMonitor
        ITaskMonitor Execution { get; }

        Task ExecuteAsync(object parameter);

        void RaiseCanExecuteChanged();
    }
}