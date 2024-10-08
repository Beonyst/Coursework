#nullable enable
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.SuppliersEditor;

public class EditSupplierViewModel : EditorPanelViewModelBase<Supplier>
{
    private readonly ISupplierService _supplierService;
    private SupplierItemViewModel _editedSupplierItemViewModel = null!;

    public SupplierItemViewModel EditedSupplierItemViewModel
    {
        get => _editedSupplierItemViewModel;
        set
        {
            _editedSupplierItemViewModel = value;
            RaisePropertyChanged(nameof(EditedSupplierItemViewModel));
        }
    }

    public EditSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public void Init(SupplierItemViewModel supplierItemViewModel)
    {
        EditedSupplierItemViewModel = new SupplierItemViewModel(supplierItemViewModel);
    }

    protected async override void Save(object? obj)
    {
        if (ValidateItem() is false)
        {
            return;
        }

        try
        {
            await _supplierService.UpdateSupplierAsync(new Supplier { Id = EditedSupplierItemViewModel.Id, Name = EditedSupplierItemViewModel.Name });
            var updatedSupplier = await _supplierService.GetAsync(EditedSupplierItemViewModel.Id);

            ClosePanel(EditorPanelResult.Success, updatedSupplier);
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
        var validationResult = EditedSupplierItemViewModel.Validate();
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
