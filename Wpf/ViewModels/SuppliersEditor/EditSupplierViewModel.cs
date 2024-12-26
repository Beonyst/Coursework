#nullable enable
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

// Модель для редактирования поставщика
public class EditSupplierViewModel : EditorPanelViewModelBase<Supplier>
{
    private readonly ISupplierService _supplierService; // Сервис для работы с данными поставщика
    private SupplierItemViewModel _editedSupplierItemViewModel = null!; // Элемент поставщика, который редактируется

    // Свойство для редактируемого поставщика
    public SupplierItemViewModel EditedSupplierItemViewModel
    {
        get => _editedSupplierItemViewModel;
        set
        {
            _editedSupplierItemViewModel = value;
            RaisePropertyChanged(nameof(EditedSupplierItemViewModel)); // Уведомление об изменении модели
        }
    }

    // Конструктор с инъекцией зависимости для сервиса поставщиков
    public EditSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // Инициализация модели редактируемого поставщика
    public void Init(SupplierItemViewModel supplierItemViewModel)
    {
        EditedSupplierItemViewModel = new SupplierItemViewModel(supplierItemViewModel); // Создание копии поставщика для редактирования
    }

    // Переопределение метода сохранения изменений в поставщике
    protected async override void Save(object? obj)
    {
        // Проверка на ошибки валидации
        if (ValidateItem() is false)
        {
            return;
        }

        try
        {
            // Обновление данных поставщика в сервисе
            await _supplierService.UpdateSupplierAsync(new Supplier { Id = EditedSupplierItemViewModel.Id, Name = EditedSupplierItemViewModel.Name });
            // Получение обновленных данных поставщика
            var updatedSupplier = await _supplierService.GetAsync(EditedSupplierItemViewModel.Id);

            // Закрытие панели с результатом успеха и передача обновленного поставщика
            ClosePanel(EditorPanelResult.Success, updatedSupplier);
            ErrorMessage = null; // Очистка сообщения об ошибке
            HasErrors = false; // Сброс флага ошибок
        }
        catch (Exception ex)
        {
            // В случае ошибки, выводим сообщение
            ShowErrorMessage(ex.Message);
        }
    }

    // Метод для валидации данных редактируемого поставщика
    private bool ValidateItem()
    {
        var validationResult = EditedSupplierItemViewModel.Validate();
        if (validationResult is not null)
        {
            var errorMessageBuilder = new StringBuilder();
            // Сбор всех сообщений об ошибках
            foreach (var result in validationResult)
            {
                errorMessageBuilder.AppendLine(result.ErrorMessage);
            }

            // Показ сообщения об ошибке
            ShowErrorMessage(errorMessageBuilder.ToString().TrimEnd('\n'));
            return false; // Возвращаем false, если есть ошибки
        }

        return true; // Если ошибок нет, возвращаем true
    }
}
