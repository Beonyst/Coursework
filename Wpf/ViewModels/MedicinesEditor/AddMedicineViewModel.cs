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

// ViewModel для добавления нового медикамента
public class AddMedicineViewModel : EditorPanelViewModelBase<Medicine>
{
    private readonly IMedicineService _medicineService; // Сервис для работы с медикаментами
    private readonly ISupplierService _supplierService; // Сервис для работы с поставщиками
    private MedicineItemViewModel _newMedicineItemViewModel = null!; // Новый медикамент
    private SupplierItemViewModel _selectedSupplier = null!; // Выбранный поставщик
    private ObservableCollection<SupplierItemViewModel> _supplierItems = null!; // Список поставщиков

    // Свойство для доступа к новому медикаменту
    public MedicineItemViewModel NewMedicineItemViewModel
    {
        get => _newMedicineItemViewModel;
        set
        {
            _newMedicineItemViewModel = value;
            RaisePropertyChanged(nameof(NewMedicineItemViewModel)); // Уведомление об изменении нового медикамента
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

    // Свойство для доступа к родительской ViewModel
    public DataBaseEditorViewModelBase ParentViewModel { get; set; } = null!;

    // Конструктор для инициализации сервисов медикаментов и поставщиков
    public AddMedicineViewModel(IMedicineService medicineService, ISupplierService supplierService)
    {
        _medicineService = medicineService;
        _supplierService = supplierService;
    }

    // Метод инициализации для создания нового медикамента и загрузки списка поставщиков
    public async void Init()
    {
        NewMedicineItemViewModel = new MedicineItemViewModel(); // Инициализация нового медикамента
        SupplierItems = new ObservableCollection<SupplierItemViewModel>(); // Инициализация списка поставщиков
        var suppliers = await _supplierService.GetAllAsync(); // Получение всех поставщиков
        foreach (var supplier in suppliers)
        {
            SupplierItems.Add(new SupplierItemViewModel(supplier)); // Добавление поставщиков в список
        }
    }

    // Метод сохранения нового медикамента
    protected async override void Save(object? obj)
    {
        if (ValidateItem() is false) // Проверка валидности данных
        {
            return;
        }

        try
        {
            // Добавление нового медикамента через сервис
            var addedMedicine = await _medicineService.AddMedicineAsync(new MedicineParameters
            {
                Name = NewMedicineItemViewModel.Name,
                Description = NewMedicineItemViewModel.Description,
                Price = NewMedicineItemViewModel.Price,
                SupplierId = SelectedSupplier.Id // Установка поставщика
            });
            NewMedicineItemViewModel = new MedicineItemViewModel(addedMedicine); // Инициализация новым добавленным медикаментом

            ClosePanel(EditorPanelResult.Success, addedMedicine); // Закрытие панели с успешным результатом
            ErrorMessage = null;
            HasErrors = false;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message); // Показ ошибки, если произошла ошибка при добавлении
        }
    }

    // Метод для валидации данных
    private bool ValidateItem()
    {
        var validationResult = NewMedicineItemViewModel.Validate(); // Получение результатов валидации
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
