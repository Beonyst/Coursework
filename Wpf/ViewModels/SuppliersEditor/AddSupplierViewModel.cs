#nullable enable
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

public class AddSupplierViewModel : EditorPanelViewModelBase<Supplier>
{
    private readonly ISupplierService _supplierService;
    private SupplierItemViewModel _newSupplierItemViewModel = null!;

    public SupplierItemViewModel NewSupplierItemViewModel
    {
        get => _newSupplierItemViewModel;
        set
        {
            _newSupplierItemViewModel = value;
            RaisePropertyChanged(nameof(NewSupplierItemViewModel));
        }
    }

    public DataBaseEditorViewModelBase ParentViewModel { get; set; } = null!;

    public AddSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public void Init()
    {
        NewSupplierItemViewModel = new SupplierItemViewModel();
    }

    protected async override void Save(object? obj)
    {
        if (ValidateItem() is false)
        {
            return;
        }

        try
        {
            var addedSupplier = await _supplierService.AddSupplierAsync(NewSupplierItemViewModel.Name);
            NewSupplierItemViewModel = new SupplierItemViewModel(addedSupplier);

            ClosePanel(EditorPanelResult.Success, addedSupplier);
            ErrorMessage = null;
            HasErrors = false;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }

    private bool ValidateItem()
    {
        var validationResult = NewSupplierItemViewModel.Validate();
        if (validationResult is not null)
        {
            var errorMessageBuilder = new StringBuilder();
            foreach (var result in validationResult)
            {
                errorMessageBuilder.AppendLine(result.ErrorMessage);
            }

            ShowErrorMessage(errorMessageBuilder.ToString().TrimEnd('\n'));
            return false;
        }

        return true;
    }
}
