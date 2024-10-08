#nullable enable
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Wpf.Infrastructure;

public abstract class ViewModelBase : IViewModel
{
    protected Dispatcher CurrentDispatcher = Application.Current.Dispatcher;

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    protected virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void RaiseCommandCanExecuteChanged(ICommandWithRaiseCanExecute command)
    {
        command.RaiseCanExecuteChanged();
    }
}
