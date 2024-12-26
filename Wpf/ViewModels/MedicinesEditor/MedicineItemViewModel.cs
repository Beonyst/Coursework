#nullable enable
using System.ComponentModel.DataAnnotations;
using Wpf.Infrastructure;
using Wpf.Models;

namespace Wpf.ViewModels.MedicinesEditor;

public class MedicineItemViewModel : ViewModelBase
{
    private string _name = null!; // Название медикамента
    private string _description = null!; // Описание медикамента
    private double _price; // Цена медикамента (в рублях и копейках)
    private int _rubles; // Рубли
    private int _kopecks; // Копейки
    private Supplier _supplier = null!; // Поставщик медикамента

    public int Id { get; } // Уникальный идентификатор медикамента

    // Свойство для имени медикамента
    [Required]
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                RaisePropertyChanged(nameof(Name)); // Уведомление об изменении имени
            }
        }
    }

    // Свойство для описания медикамента
    [Required]
    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                RaisePropertyChanged(nameof(Description)); // Уведомление об изменении описания
            }
        }
    }

    // Свойство для рублей
    [Required]
    public int Rubles
    {
        get => _rubles;
        set
        {
            if (_rubles != value)
            {
                _rubles = value;
                RaisePropertyChanged(nameof(Rubles)); // Уведомление об изменении рублей
                RaisePropertyChanged(nameof(Price)); // Уведомление об изменении цены
            }
        }
    }

    // Свойство для копеек
    [Required]
    public int Kopecks
    {
        get => _kopecks;
        set
        {
            if (_kopecks != value)
            {
                _kopecks = value;
                RaisePropertyChanged(nameof(Kopecks)); // Уведомление об изменении копеек
                RaisePropertyChanged(nameof(Price)); // Уведомление об изменении цены
            }
        }
    }

    // Свойство для вычисляемой цены (рубли + копейки)
    [Required]
    public double Price
    { 
        get => _rubles + Kopecks / 100d; // Цена в рублях
        set { } // Свойство только для чтения
    }

    // Свойство для поставщика
    public Supplier Supplier
    {
        get => _supplier;
        set
        {
            if (_supplier != value)
            {
                _supplier = value;
                RaisePropertyChanged(nameof(Supplier)); // Уведомление об изменении поставщика
            }
        }
    }

    // Конструктор по умолчанию
    public MedicineItemViewModel()
    {

    }

    // Конструктор с использованием объекта Medicine
    public MedicineItemViewModel(Medicine medicine)
    {
        Id = medicine.Id;
        Name = medicine.Name;
        Description = medicine.Description;
        Rubles = (int)Math.Floor(medicine.Price); // Извлекаем рубли из цены
        Kopecks = (int)(medicine.Price * 100 % 100); // Извлекаем копейки из цены
        Supplier = medicine.Supplier;
    }

    // Конструктор с использованием другого объекта MedicineItemViewModel
    public MedicineItemViewModel(MedicineItemViewModel medicineItemViewModel)
    {
        Id = medicineItemViewModel.Id;
        Name = medicineItemViewModel.Name;
        Description = medicineItemViewModel.Description;
        Rubles = (int)Math.Floor(medicineItemViewModel.Price); // Извлекаем рубли из цены
        Kopecks = (int)(medicineItemViewModel.Price * 100 % 100); // Извлекаем копейки из цены
        Supplier = medicineItemViewModel.Supplier;
    }

    // Метод для валидации данных
    public ICollection<ValidationResult>? Validate()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(this, context, results, true) is false)
        {
            return results; // Возвращаем список ошибок валидации
        }

        return null; // Если ошибок нет, возвращаем null
    }
}
