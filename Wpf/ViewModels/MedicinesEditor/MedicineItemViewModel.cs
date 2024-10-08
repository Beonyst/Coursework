#nullable enable
using System.ComponentModel.DataAnnotations;
using Wpf.Infrastructure;
using Wpf.Models;

namespace Wpf.ViewModels.MedicinesEditor;

public class MedicineItemViewModel : ViewModelBase
{
    private string _name = null!;
    private string _description = null!;
    private double _price;
    private int _rubles;
    private int _kopecks;
    private Supplier _supplier = null!;

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

    [Required]
    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
    }

    [Required]
    public int Rubles
    {
        get => _rubles;
        set
        {
            if (_rubles != value)
            {
                _rubles = value;
                RaisePropertyChanged(nameof(Rubles));
                RaisePropertyChanged(nameof(Price));
            }
        }
    }

    [Required]
    public int Kopecks
    {
        get => _kopecks;
        set
        {
            if (_kopecks != value)
            {
                _kopecks = value;
                RaisePropertyChanged(nameof(Kopecks));
                RaisePropertyChanged(nameof(Price));
            }
        }
    }

    [Required]
    public double Price
    { 
        get => _rubles + Kopecks / 100d;
        set { }
    }


    public Supplier Supplier
    {
        get => _supplier;
        set
        {
            if (_supplier != value)
            {
                _supplier = value;
                RaisePropertyChanged(nameof(Supplier));
            }
        }
    }

    public MedicineItemViewModel()
    {

    }

    public MedicineItemViewModel(Medicine medicine)
    {
        Id = medicine.Id;
        Name = medicine.Name;
        Description = medicine.Description;
        Rubles = (int)Math.Floor(medicine.Price);
        Kopecks = (int)(medicine.Price * 100 % 100);
        Supplier = medicine.Supplier;
    }

    public MedicineItemViewModel(MedicineItemViewModel medicineItemViewModel)
    {
        Id = medicineItemViewModel.Id;
        Name = medicineItemViewModel.Name;
        Description = medicineItemViewModel.Description;
        Rubles = (int)Math.Floor(medicineItemViewModel.Price);
        Kopecks = (int)(medicineItemViewModel.Price * 100 % 100);
        Supplier = medicineItemViewModel.Supplier;
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
