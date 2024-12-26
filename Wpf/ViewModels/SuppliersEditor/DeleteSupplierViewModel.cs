#nullable enable
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

// Модель для удаления поставщика
public class DeleteSupplierViewModel : EditorPanelViewModelBase<Supplier>
{
    private readonly ISupplierService _supplierService; // Сервис для работы с поставщиками
    private string _message = null!; // Сообщение, которое отображается перед удалением

    // Элемент поставщика, который будет удален
    public SupplierItemViewModel DeletedSupplierItem { get; set; } = null!;

    // Сообщение, которое отображается в интерфейсе
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            RaisePropertyChanged(nameof(Message)); // Уведомление о смене сообщения
        }
    }

    // Конструктор с инъекцией зависимости для сервиса поставщиков
    public DeleteSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // Инициализация модели для удаления поставщика
    public void Init(SupplierItemViewModel supplierItemViewModel)
    {
        DeletedSupplierItem = supplierItemViewModel; // Установка поставщика для удаления
        Message = $"Удалить поставщика {DeletedSupplierItem.Name}?"; // Формирование сообщения
    }

    // Переопределение метода сохранения, который выполняет удаление
    protected override async void Save(object? obj)
    {
        try
        {
            // Удаление поставщика через сервис
            await _supplierService.DeleteSupplier(DeletedSupplierItem.Id);
            // Закрытие панели с успешным результатом
            ClosePanel(EditorPanelResult.Success, null);
        }
        catch (Exception ex)
        {
            // В случае ошибки, выводим сообщение об ошибке
            ShowErrorMessage(ex.Message);
        }
    }
}
