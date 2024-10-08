#nullable enable
using System.Collections.ObjectModel;
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Models.ApiRequestModels;
using Wpf.Services.Interfaces;
using Wpf.ViewModels.SuppliersEditor;

namespace Wpf.ViewModels.MedicinesEditor;

public class EditMedicineViewModel : EditorPanelViewModelBase<Medicine>
{
    private readonly IMedicineService _medicineService;
    private readonly ISupplierService _supplierService;
    private SupplierItemViewModel _selectedSupplier = null!;
    private MedicineItemViewModel _editedMedicineItemViewModel = null!;
    private ObservableCollection<SupplierItemViewModel> _supplierItems = null!;

    public MedicineItemViewModel EditedMedicineItemViewModel
    {
        get => _editedMedicineItemViewModel;
        set
        {
            _editedMedicineItemViewModel = value;
            RaisePropertyChanged(nameof(EditedMedicineItemViewModel));
        }
    }

    public SupplierItemViewModel SelectedSupplier
    {
        get => _selectedSupplier;
        set
        {
            if (_selectedSupplier != value)
            {
                _selectedSupplier = value;
                RaisePropertyChanged(nameof(SelectedSupplier));
            }
        }
    }

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

    public EditMedicineViewModel(IMedicineService medicineService, ISupplierService supplierService)
    {
        _medicineService = medicineService;
        _supplierService = supplierService;
    }

    public async void Init(MedicineItemViewModel supplierItemViewModel)
    {
        EditedMedicineItemViewModel = new MedicineItemViewModel(supplierItemViewModel);
        SupplierItems = new ObservableCollection<SupplierItemViewModel>();
        var suppliers = await _supplierService.GetAllAsync();
        foreach (var supplier in suppliers)
        {
            SupplierItems.Add(new SupplierItemViewModel(supplier));
        }
        SelectedSupplier = SupplierItems.First(supplier => supplier.Id == EditedMedicineItemViewModel.Supplier.Id);
    }

    protected override async void Save(object? obj)
    {
        if (ValidateItem() is false)
        {
            return;
        }

        try
        {
            await _medicineService.UpdateMedicineAsync(EditedMedicineItemViewModel.Id, new MedicineParameters
            {
                Name = EditedMedicineItemViewModel.Name,
                Description = EditedMedicineItemViewModel.Description,
                Price = EditedMedicineItemViewModel.Price,
                SupplierId = SelectedSupplier.Id
            });
            var updatedMedicine = await _medicineService.GetAsync(EditedMedicineItemViewModel.Id);
            updatedMedicine.Supplier = new Supplier 
            { 
                Id = SelectedSupplier.Id ,
                Name = SelectedSupplier.Name
            };
            ClosePanel(EditorPanelResult.Success, updatedMedicine);
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
        var validationResult = EditedMedicineItemViewModel.Validate();
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
