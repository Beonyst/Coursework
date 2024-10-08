#nullable enable
using System.Collections.ObjectModel;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

public class SuppliersEditorViewModel : DataBaseEditorViewModelBase
{
    private readonly ISupplierService _supplierService;
    private readonly AddSupplierViewModel _addSupplierViewModel;
    private readonly EditSupplierViewModel _editSupplierViewModel;
    private ObservableCollection<SupplierItemViewModel> _supplierItems = null!;
    private SupplierItemViewModel? _selectedSupplierItem;
    private EditorPanelViewModelBase<Supplier> _editorPanelViewModel;
    private bool _isDeletePanelVisible;

    public ObservableCollection<SupplierItemViewModel> SupplierItems
    {
        get => _supplierItems;
        set
        {
            if (_supplierItems != value)
            {
                _supplierItems = value;
                RaisePropertyChanged(nameof(SupplierItems));
            }
        }
    }

    public SupplierItemViewModel? SelectedSupplierItem
    {
        get => _selectedSupplierItem;
        set
        {
            if (_selectedSupplierItem != value)
            {
                _selectedSupplierItem = value;
                RaisePropertyChanged(nameof(SelectedSupplierItem));
                RaiseCommandCanExecuteChanged(ShowEditItemPanelCmd);
                RaiseCommandCanExecuteChanged(ShowDeleteItemPanelCmd);
            }
        }
    }

    public EditorPanelViewModelBase<Supplier> EditorPanelViewModel
    {
        get => _editorPanelViewModel;
        set
        {
            if (_editorPanelViewModel != value)
            {
                _editorPanelViewModel = value;
                RaisePropertyChanged(nameof(EditorPanelViewModel));
            }
        }
    }

    public DeleteSupplierViewModel DeleteSupplierViewModel { get; set; }

    public bool IsDeletePanelVisible
    {
        get => _isDeletePanelVisible;
        set
        {
            if (_isDeletePanelVisible != value)
            {
                _isDeletePanelVisible = value;
                RaisePropertyChanged(nameof(IsDeletePanelVisible));
            }
        }
    }

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

    public override async void Update()
    {
        var suppliers = await _supplierService.GetAllAsync();
        SupplierItems = new ObservableCollection<SupplierItemViewModel>(suppliers.Select(supplier => new SupplierItemViewModel(supplier)));
    }

    protected override void RefreshData(object? obj)
    {
        Update();
    }

    protected override void ShowAddItemPanel(object? obj)
    {
        _addSupplierViewModel.Init();
        EditorPanelViewModel = _addSupplierViewModel;
        base.ShowAddItemPanel(obj);
    }

    protected override void ShowDeleteItemPanel(object? obj)
    {
        DeleteSupplierViewModel.Init(SelectedSupplierItem);
        IsDeletePanelVisible = true;
    }

    protected override bool CanExecuteShowDeleteItemPanelCmd(object? obj)
    {
        return SelectedSupplierItem is not null;
    }

    protected override void ShowEditItemPanel(object? obj)
    {
        _editSupplierViewModel.Init(SelectedSupplierItem!);
        EditorPanelViewModel = _editSupplierViewModel;
        base.ShowEditItemPanel(obj);
    }

    protected override bool CanExecuteShowEditItemPanelCmd(object? obj)
    {
        return SelectedSupplierItem is not null;
    }

    protected override bool CanExecuteEditorCommands(object? obj)
    {
        return IsEditorPanelVisible is false && IsDeletePanelVisible is false;
    }

    private void AddSupplierViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Supplier> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            SupplierItems.Add(new SupplierItemViewModel(editorPanelClosedEventArgs.EditedModel));
        }

        IsEditorPanelVisible = false;
    }

    private void EditSupplierViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Supplier> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            var supplierToReplace = SupplierItems.FirstOrDefault(supplier => supplier.Id == editorPanelClosedEventArgs.EditedModel.Id);
            if (supplierToReplace is not null)
            {
                var index = SupplierItems.IndexOf(supplierToReplace);
                SupplierItems.RemoveAt(index);
                SupplierItems.Insert(index, new SupplierItemViewModel(editorPanelClosedEventArgs.EditedModel));
            }
        }

        IsEditorPanelVisible = false;
    }

    private void DeleteSupplierViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Supplier> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            SupplierItems.Remove(DeleteSupplierViewModel.DeletedSupplierItem);
        }

        IsDeletePanelVisible = false;
    }
}
