#nullable enable
using System.Collections.ObjectModel;
using System.Text;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Models.ApiRequestModels;
using Wpf.Services.Interfaces;
using Wpf.ViewModels.SuppliersEditor;

namespace Wpf.ViewModels.MedicinesEditor;

public class AddMedicineViewModel : EditorPanelViewModelBase<Medicine>
{
    private readonly IMedicineService _medicineService;
    private readonly ISupplierService _supplierService;
    private MedicineItemViewModel _newMedicineItemViewModel = null!;
    private SupplierItemViewModel _selectedSupplier = null!;
    private ObservableCollection<SupplierItemViewModel> _supplierItems = null!;

    public MedicineItemViewModel NewMedicineItemViewModel
    {
        get => _newMedicineItemViewModel;
        set
        {
            _newMedicineItemViewModel = value;
            RaisePropertyChanged(nameof(NewMedicineItemViewModel));
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

    public DataBaseEditorViewModelBase ParentViewModel { get; set; } = null!;

    public AddMedicineViewModel(IMedicineService medicineService, ISupplierService supplierService)
    {
        _medicineService = medicineService;
        _supplierService = supplierService;
    }

    public async void Init()
    {
        NewMedicineItemViewModel = new MedicineItemViewModel();
        SupplierItems = new ObservableCollection<SupplierItemViewModel>();
        var suppliers = await _supplierService.GetAllAsync();
        foreach (var supplier in suppliers)
        {
            SupplierItems.Add(new SupplierItemViewModel(supplier));
        }
    }

    protected async override void Save(object? obj)
    {
        if (ValidateItem() is false)
        {
            return;
        }

        try
        {
            var addedMedicine =  await _medicineService.AddMedicineAsync(new MedicineParameters
            {
                Name = NewMedicineItemViewModel.Name,
                Description = NewMedicineItemViewModel.Description,
                Price = NewMedicineItemViewModel.Price,
                SupplierId = SelectedSupplier.Id
            });
            NewMedicineItemViewModel = new MedicineItemViewModel(addedMedicine);

            ClosePanel(EditorPanelResult.Success, addedMedicine);
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
        var validationResult = NewMedicineItemViewModel.Validate();
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
