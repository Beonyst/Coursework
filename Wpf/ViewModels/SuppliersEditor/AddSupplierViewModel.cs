#nullable enable
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

// Модель для добавления нового поставщика
public class AddSupplierViewModel : EditorPanelViewModelBase<Supplier>
{
    private readonly ISupplierService _supplierService; // Сервис для работы с поставщиками
    private SupplierItemViewModel _newSupplierItemViewModel = null!; // Новый поставщик, который добавляется

    // Свойство для отображения нового поставщика
    public SupplierItemViewModel NewSupplierItemViewModel
    {
        get => _newSupplierItemViewModel;
        set
        {
            _newSupplierItemViewModel = value;
            RaisePropertyChanged(nameof(NewSupplierItemViewModel)); // Уведомление об изменении свойства
        }
    }

    // Родительская модель, которая вызывает добавление
    public DataBaseEditorViewModelBase ParentViewModel { get; set; } = null!;

    // Конструктор с инъекцией зависимости для сервиса поставщиков
    public AddSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // Инициализация модели для добавления нового поставщика
    public void Init()
    {
        NewSupplierItemViewModel = new SupplierItemViewModel(); // Создание пустого объекта поставщика
    }

    // Переопределение метода сохранения нового поставщика
    protected async override void Save(object? obj)
    {
        if (ValidateItem() is false)
        {
            return; // Если валидация не прошла, выходим
        }

        try
        {
            // Добавление нового поставщика через сервис
            var addedSupplier = await _supplierService.AddSupplierAsync(NewSupplierItemViewModel.Name);
            // Инициализация нового поставщика для отображения
            NewSupplierItemViewModel = new SupplierItemViewModel(addedSupplier);

            // Закрытие панели с успешным результатом
            ClosePanel(EditorPanelResult.Success, addedSupplier);
            ErrorMessage = null; // Очистка сообщения об ошибке
            HasErrors = false; // Нет ошибок
        }
        catch (Exception ex)
        {
            // В случае ошибки, выводим сообщение об ошибке
            ShowErrorMessage(ex.Message);
        }
    }

    // Валидация нового поставщика
    private bool ValidateItem()
    {
        var validationResult = NewSupplierItemViewModel.Validate();
        if (validationResult is not null)
        {
            var errorMessageBuilder = new StringBuilder();
            foreach (var result in validationResult)
            {
                errorMessageBuilder.AppendLine(result.ErrorMessage); // Сбор всех ошибок
            }

            ShowErrorMessage(errorMessageBuilder.ToString().TrimEnd('\n')); // Отображение ошибок
            return false;
        }

        return true; // Если ошибок нет, возвращаем true
    }
}
