#nullable enable
using System.Collections.ObjectModel;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

// Класс SuppliersEditorViewModel управляет панелью редактора для поставщиков и взаимодействует с сервисом поставок для получения и обновления данных.
public class SuppliersEditorViewModel : DataBaseEditorViewModelBase
{
    private readonly ISupplierService _supplierService;
    private readonly AddSupplierViewModel _addSupplierViewModel;
    private readonly EditSupplierViewModel _editSupplierViewModel;
    private ObservableCollection<SupplierItemViewModel> _supplierItems = null!;
    private SupplierItemViewModel? _selectedSupplierItem;
    private EditorPanelViewModelBase<Supplier> _editorPanelViewModel;
    private bool _isDeletePanelVisible;

    // Свойство для получения или установки списка поставщиков
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

    // Свойство для получения или установки выбранного поставщика
    public SupplierItemViewModel? SelectedSupplierItem
    {
        get => _selectedSupplierItem;
        set
        {
            if (_selectedSupplierItem != value)
            {
                _selectedSupplierItem = value;
                RaisePropertyChanged(nameof(SelectedSupplierItem)); // Уведомление об изменении выбранного поставщика
                RaiseCommandCanExecuteChanged(ShowEditItemPanelCmd); // Обновление доступности команд
                RaiseCommandCanExecuteChanged(ShowDeleteItemPanelCmd); // Обновление доступности команд
            }
        }
    }

    // Свойство для получения или установки панели редактора
    public EditorPanelViewModelBase<Supplier> EditorPanelViewModel
    {
        get => _editorPanelViewModel;
        set
        {
            if (_editorPanelViewModel != value)
            {
                _editorPanelViewModel = value;
                RaisePropertyChanged(nameof(EditorPanelViewModel)); // Уведомление об изменении панели редактора
            }
        }
    }

    // Свойство для получения или установки панели удаления поставщика
    public DeleteSupplierViewModel DeleteSupplierViewModel { get; set; }

    // Свойство для получения или установки видимости панели удаления
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

    // Конструктор, инициализирующий сервис поставщиков и другие представления
    public SuppliersEditorViewModel(ISupplierService supplierService, DeleteSupplierViewModel deleteSupplierViewModel,
        AddSupplierViewModel addSupplierViewModel, EditSupplierViewModel editSupplierViewModel) : base("Поставщики")
    {
        _supplierService = supplierService;

        _addSupplierViewModel = addSupplierViewModel;
        _addSupplierViewModel.ParentViewModel = this;
        _addSupplierViewModel.EditorPanelClosed += AddSupplierViewModelOnEditorPanelClosed;

        _editSupplierViewModel = editSupplierViewModel;
        _editSupplierViewModel.EditorPanelClosed += EditSupplierViewModelOnEditorPanelClosed;

        DeleteSupplierViewModel = deleteSupplierViewModel;
        DeleteSupplierViewModel.EditorPanelClosed += DeleteSupplierViewModelOnEditorPanelClosed;
    }

    // Метод для обновления списка поставщиков
    public override async void Update()
    {
        var suppliers = await _supplierService.GetAllAsync();
        SupplierItems = new ObservableCollection<SupplierItemViewModel>(suppliers.Select(supplier => new SupplierItemViewModel(supplier)));
    }

    // Метод для обновления данных (вызывает метод Update)
    protected override void RefreshData(object? obj)
    {
        Update();
    }

    // Метод для отображения панели добавления поставщика
    protected override void ShowAddItemPanel(object? obj)
    {
        _addSupplierViewModel.Init();
        EditorPanelViewModel = _addSupplierViewModel; // Установка панели добавления
        base.ShowAddItemPanel(obj);
    }

    // Метод для отображения панели удаления поставщика
    protected override void ShowDeleteItemPanel(object? obj)
    {
        DeleteSupplierViewModel.Init(SelectedSupplierItem); // Инициализация панели удаления с выбранным поставщиком
        IsDeletePanelVisible = true;
    }

    // Метод для проверки возможности отображения панели удаления
    protected override bool CanExecuteShowDeleteItemPanelCmd(object? obj)
    {
        return SelectedSupplierItem is not null; // Панель удаления доступна, если выбран поставщик
    }

    // Метод для отображения панели редактирования поставщика
    protected override void ShowEditItemPanel(object? obj)
    {
        _editSupplierViewModel.Init(SelectedSupplierItem!); // Инициализация панели редактирования с выбранным поставщиком
        EditorPanelViewModel = _editSupplierViewModel;
        base.ShowEditItemPanel(obj);
    }

    // Метод для проверки возможности отображения панели редактирования
    protected override bool CanExecuteShowEditItemPanelCmd(object? obj)
    {
        return SelectedSupplierItem is not null; // Панель редактирования доступна, если выбран поставщик
    }

    // Метод для проверки доступности команд редактора
    protected override bool CanExecuteEditorCommands(object? obj)
    {
        return IsEditorPanelVisible is false && IsDeletePanelVisible is false; // Команды доступны, если ни одна из панелей не видна
    }

    // Обработчик закрытия панели добавления поставщика
    private void AddSupplierViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Supplier> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            SupplierItems.Add(new SupplierItemViewModel(editorPanelClosedEventArgs.EditedModel)); // Добавление нового поставщика в список
        }

        IsEditorPanelVisible = false;
    }

    // Обработчик закрытия панели редактирования поставщика
    private void EditSupplierViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Supplier> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            var supplierToReplace = SupplierItems.FirstOrDefault(supplier => supplier.Id == editorPanelClosedEventArgs.EditedModel.Id);
            if (supplierToReplace is not null)
            {
                var index = SupplierItems.IndexOf(supplierToReplace);
                SupplierItems.RemoveAt(index); // Замена редактируемого поставщика
                SupplierItems.Insert(index, new SupplierItemViewModel(editorPanelClosedEventArgs.EditedModel));
            }
        }

        IsEditorPanelVisible = false;
    }

    // Обработчик закрытия панели удаления поставщика
    private void DeleteSupplierViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Supplier> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            SupplierItems.Remove(DeleteSupplierViewModel.DeletedSupplierItem); // Удаление поставщика из списка
        }

        IsDeletePanelVisible = false;
    }
}
