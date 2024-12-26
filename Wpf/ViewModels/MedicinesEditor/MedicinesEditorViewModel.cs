#nullable enable
using System.Collections.ObjectModel;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.MedicinesEditor;

public class MedicinesEditorViewModel : DataBaseEditorViewModelBase
{
    private readonly IMedicineService _medicineService; // Сервис для работы с медикаментами
    private readonly ISupplierService _supplierService; // Сервис для работы с поставщиками
    private readonly AddMedicineViewModel _addMedicineViewModel; // Модель для добавления медикамента
    private readonly EditMedicineViewModel _editMedicineViewModel; // Модель для редактирования медикамента
    private ObservableCollection<MedicineItemViewModel> _medicineItems = null!; // Список медикаментов
    private MedicineItemViewModel? _selectedMedicineItem; // Выбранный медикамент
    private EditorPanelViewModelBase<Medicine> _editorPanelViewModel; // Панель редактирования медикамента
    private bool _isDeletePanelVisible; // Видимость панели удаления

    // Свойство для отображения списка медикаментов
    public ObservableCollection<MedicineItemViewModel> MedicineItems
    {
        get => _medicineItems;
        set
        {
            if (_medicineItems != value)
            {
                _medicineItems = value;
                RaisePropertyChanged(nameof(MedicineItems)); // Уведомление об изменении списка медикаментов
            }
        }
    }

    // Свойство для выбора медикамента
    public MedicineItemViewModel? SelectedMedicineItem
    {
        get => _selectedMedicineItem;
        set
        {
            if (_selectedMedicineItem != value)
            {
                _selectedMedicineItem = value;
                RaisePropertyChanged(nameof(SelectedMedicineItem)); // Уведомление об изменении выбранного медикамента
                RaiseCommandCanExecuteChanged(ShowEditItemPanelCmd); // Обновление состояния команды редактирования
                RaiseCommandCanExecuteChanged(ShowDeleteItemPanelCmd); // Обновление состояния команды удаления
            }
        }
    }

    // Свойство для отображения панели редактирования
    public EditorPanelViewModelBase<Medicine> EditorPanelViewModel
    {
        get => _editorPanelViewModel;
        set
        {
            if (_editorPanelViewModel != value)
            {
                _editorPanelViewModel = value;
                RaisePropertyChanged(nameof(EditorPanelViewModel)); // Уведомление об изменении панели редактирования
            }
        }
    }

    // Модель для удаления медикамента
    public DeleteMedicineViewModel DeleteMedicineViewModel { get; set; }

    // Свойство для управления видимостью панели удаления
    public bool IsDeletePanelVisible
    {
        get => _isDeletePanelVisible;
        set
        {
            if (_isDeletePanelVisible != value)
            {
                _isDeletePanelVisible = value;
                RaisePropertyChanged(nameof(IsDeletePanelVisible)); // Уведомление об изменении видимости панели удаления
            }
        }
    }

    // Конструктор с инъекцией зависимостей
    public MedicinesEditorViewModel(IMedicineService medicineService, ISupplierService supplierService, DeleteMedicineViewModel deleteMedicineViewModel,
        AddMedicineViewModel addMedicineViewModel, EditMedicineViewModel editMedicineViewModel) : base("Медикаменты")
    {
        _medicineService = medicineService;
        _supplierService = supplierService;

        // Инициализация моделей для добавления и редактирования медикаментов
        _addMedicineViewModel = addMedicineViewModel;
        _addMedicineViewModel.ParentViewModel = this;
        _addMedicineViewModel.EditorPanelClosed += AddMedicineViewModelOnEditorPanelClosed;

        _editMedicineViewModel = editMedicineViewModel;
        _editMedicineViewModel.EditorPanelClosed += EditMedicineViewModelOnEditorPanelClosed;

        // Инициализация модели для удаления медикаментов
        DeleteMedicineViewModel = deleteMedicineViewModel;
        DeleteMedicineViewModel.EditorPanelClosed += DeleteMedicineViewModelOnEditorPanelClosed;
    }

    // Метод для обновления списка медикаментов
    public override async void Update()
    {
        var medicines = await _medicineService.GetAllAsync();
        MedicineItems = new ObservableCollection<MedicineItemViewModel>(medicines.Select(medicine => new MedicineItemViewModel(medicine)));
    }

    // Метод для обновления данных
    protected override void RefreshData(object? obj)
    {
        Update();
    }

    // Метод для отображения панели добавления медикамента
    protected override void ShowAddItemPanel(object? obj)
    {
        _addMedicineViewModel.Init();
        EditorPanelViewModel = _addMedicineViewModel;
        base.ShowAddItemPanel(obj); // Вызов базового метода для отображения панели добавления
    }

    // Метод для отображения панели удаления медикамента
    protected override void ShowDeleteItemPanel(object? obj)
    {
        DeleteMedicineViewModel.Init(SelectedMedicineItem);
        IsDeletePanelVisible = true;
    }

    // Условие для выполнения команды удаления
    protected override bool CanExecuteShowDeleteItemPanelCmd(object? obj)
    {
        return SelectedMedicineItem is not null; // Команда доступна, если выбран медикамент
    }

    // Метод для отображения панели редактирования медикамента
    protected override void ShowEditItemPanel(object? obj)
    {
        _editMedicineViewModel.Init(SelectedMedicineItem!);
        EditorPanelViewModel = _editMedicineViewModel;
        base.ShowEditItemPanel(obj); // Вызов базового метода для отображения панели редактирования
    }

    // Условие для выполнения команды редактирования
    protected override bool CanExecuteShowEditItemPanelCmd(object? obj)
    {
        return SelectedMedicineItem is not null; // Команда доступна, если выбран медикамент
    }

    // Условие для выполнения команд редактирования и удаления
    protected override bool CanExecuteEditorCommands(object? obj)
    {
        return IsEditorPanelVisible is false && IsDeletePanelVisible is false; // Команды доступны, если не отображаются панели редактирования или удаления
    }

    // Обработчик закрытия панели добавления медикамента
    private void AddMedicineViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Medicine> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            MedicineItems.Add(new MedicineItemViewModel(editorPanelClosedEventArgs.EditedModel)); // Добавляем новый медикамент в список
        }

        IsEditorPanelVisible = false; // Скрываем панель редактирования
    }

    // Обработчик закрытия панели редактирования медикамента
    private void EditMedicineViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Medicine> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            var medicineToReplace = MedicineItems.FirstOrDefault(medicine => medicine.Id == editorPanelClosedEventArgs.EditedModel.Id);
            if (medicineToReplace is not null)
            {
                var index = MedicineItems.IndexOf(medicineToReplace);
                MedicineItems.RemoveAt(index);
                MedicineItems.Insert(index, new MedicineItemViewModel(editorPanelClosedEventArgs.EditedModel)); // Обновляем медикамент в списке
            }
        }

        IsEditorPanelVisible = false; // Скрываем панель редактирования
    }

    // Обработчик закрытия панели удаления медикамента
    private void DeleteMedicineViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Medicine> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            MedicineItems.Remove(DeleteMedicineViewModel.DeletedMedicineItem); // Удаляем медикамент из списка
        }

        IsDeletePanelVisible = false; // Скрываем панель удаления
    }
}
