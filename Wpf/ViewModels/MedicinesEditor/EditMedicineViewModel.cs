#nullable enable

// Подключение необходимых пространств имен
using System.Collections.ObjectModel;
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Models.ApiRequestModels;
using Wpf.Services.Interfaces;
using Wpf.ViewModels.SuppliersEditor;

namespace Wpf.ViewModels.MedicinesEditor;

// ViewModel для редактирования медикаментов
public class EditMedicineViewModel : EditorPanelViewModelBase<Medicine>
{
    private readonly IMedicineService _medicineService; // Сервис для работы с медикаментами
    private readonly ISupplierService _supplierService; // Сервис для работы с поставщиками
    private SupplierItemViewModel _selectedSupplier = null!; // Выбранный поставщик
    private MedicineItemViewModel _editedMedicineItemViewModel = null!; // Редактируемый медикамент
    private ObservableCollection<SupplierItemViewModel> _supplierItems = null!; // Список поставщиков

    // Свойство для доступа к редактируемому медикаменту
    public MedicineItemViewModel EditedMedicineItemViewModel
    {
        get => _editedMedicineItemViewModel;
        set
        {
            _editedMedicineItemViewModel = value;
            RaisePropertyChanged(nameof(EditedMedicineItemViewModel)); // Уведомление об изменении редактируемого медикамента
        }
    }

    // Свойство для доступа к выбранному поставщику
    public SupplierItemViewModel SelectedSupplier
    {
        get => _selectedSupplier;
        set
        {
            if (_selectedSupplier != value)
            {
                _selectedSupplier = value;
                RaisePropertyChanged(nameof(SelectedSupplier)); // Уведомление об изменении выбранного поставщика
            }
        }
    }

    // Свойство для доступа к списку поставщиков
    public ObservableCollection<SupplierItemViewModel> SupplierItems
    {
        get => _supplierItems;
        set
        {
            if (_supplierItems != value)
            {
                _supplierItems = value;
                RaisePropertyChanged(nameof(SupplierItems)); // Уведомление об изменении списка поставщиков
            }
        }
    }

    // Конструктор для инициализации сервисов медикаментов и поставщиков
    public EditMedicineViewModel(IMedicineService medicineService, ISupplierService supplierService)
    {
        _medicineService = medicineService;
        _supplierService = supplierService;
    }

    // Метод инициализации для установки редактируемого медикамента и списка поставщиков
    public async void Init(MedicineItemViewModel medicineItemViewModel)
    {
        EditedMedicineItemViewModel = new MedicineItemViewModel(medicineItemViewModel); // Инициализация редактируемого медикамента
        SupplierItems = new ObservableCollection<SupplierItemViewModel>(); // Инициализация списка поставщиков
        var suppliers = await _supplierService.GetAllAsync(); // Получение всех поставщиков
        foreach (var supplier in suppliers)
        {
            SupplierItems.Add(new SupplierItemViewModel(supplier)); // Добавление поставщиков в список
        }
        SelectedSupplier = SupplierItems.First(supplier => supplier.Id == EditedMedicineItemViewModel.Supplier.Id); // Установка выбранного поставщика
    }

    // Метод сохранения изменений медикамента
    protected override async void Save(object? obj)
    {
        if (ValidateItem() is false) // Проверка валидности данных
        {
            return;
        }

        try
        {
            // Обновление медикамента через сервис
            await _medicineService.UpdateMedicineAsync(EditedMedicineItemViewModel.Id, new MedicineParameters
            {
                Name = EditedMedicineItemViewModel.Name,
                Description = EditedMedicineItemViewModel.Description,
                Price = EditedMedicineItemViewModel.Price,
                SupplierId = SelectedSupplier.Id // Установка нового поставщика
            });
            var updatedMedicine = await _medicineService.GetAsync(EditedMedicineItemViewModel.Id); // Получение обновленного медикамента
            updatedMedicine.Supplier = new Supplier 
            { 
                Id = SelectedSupplier.Id,
                Name = SelectedSupplier.Name
            };
            // Закрытие панели с успешным результатом
            ClosePanel(EditorPanelResult.Success, updatedMedicine);
            ErrorMessage = null;
            HasErrors = false;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message); // Показ ошибки, если произошла ошибка при сохранении
        }
    }

    // Метод для валидации данных
    private bool ValidateItem()
    {
        var validationResult = EditedMedicineItemViewModel.Validate(); // Получение результатов валидации
        if (validationResult is not null)
        {
            var errorMessageBuilder = new StringBuilder();
            foreach (var result in validationResult)
            {
                errorMessageBuilder.AppendLine(result.ErrorMessage); // Сбор ошибок валидации
            }

            ShowErrorMessage(errorMessageBuilder.ToString().TrimEnd('\n')); // Показ сообщений об ошибке
            return false;
        }

        return true; // Если ошибок нет, возвращаем true
    }
}
