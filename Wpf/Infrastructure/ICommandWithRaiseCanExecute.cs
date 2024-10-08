#nullable enable
using System.Windows.Input;

namespace Wpf.Infrastructure;

public interface ICommandWithRaiseCanExecute : ICommand
{
    void RaiseCanExecuteChanged();
}
