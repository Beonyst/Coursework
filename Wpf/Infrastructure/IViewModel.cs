// Интерфейс IViewModel расширяет INotifyPropertyChanged и служит базой для всех ViewModel в приложении, 
// обеспечивая возможность уведомлять об изменении свойств.

#nullable enable
using System.ComponentModel;

namespace Wpf.Infrastructure;

// Интерфейс, который наследует INotifyPropertyChanged для уведомления об изменении свойств в ViewModel.
public interface IViewModel : INotifyPropertyChanged
{
}
