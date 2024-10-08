#nullable enable
using System.ComponentModel.DataAnnotations;
using Wpf.Infrastructure;
using Wpf.Models;

namespace Wpf.ViewModels.SuppliersEditor;

public class SupplierItemViewModel : ViewModelBase
{
    private string _name = null!;
    private bool _isSelected;

    public int Id { get; }

    [Required]
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }
    }

    public SupplierItemViewModel()
    {

    }

    public SupplierItemViewModel(Supplier supplier)
    {
        Id = supplier.Id;
        Name = supplier.Name;
        IsSelected = false;
    }

    public SupplierItemViewModel(SupplierItemViewModel supplierItemViewModel)
    {
        Id = supplierItemViewModel.Id;
        Name = supplierItemViewModel.Name;
        IsSelected = false;
    }

    public ICollection<ValidationResult>? Validate()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(this, context, results, true) is false)
        {
            return results;
        }

        return null;
    }
}
