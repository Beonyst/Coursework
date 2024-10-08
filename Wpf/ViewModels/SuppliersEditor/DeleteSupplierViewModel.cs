#nullable enable
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

public class DeleteSupplierViewModel : EditorPanelViewModelBase<Supplier>
{
    private readonly ISupplierService _supplierService;
    private string _message = null!;

    public SupplierItemViewModel DeletedSupplierItem { get; set; } = null!;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            RaisePropertyChanged(nameof(Message));
        }
    }

    public DeleteSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public void Init(SupplierItemViewModel supplierItemViewModel)
    {
        DeletedSupplierItem = supplierItemViewModel;
        Message = $"Удалить поставщика {DeletedSupplierItem.Name}?";
    }

    protected override async void Save(object? obj)
    {
        try
        {
            await _supplierService.DeleteSupplier(DeletedSupplierItem.Id);
            ClosePanel(EditorPanelResult.Success, null);
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }
}
