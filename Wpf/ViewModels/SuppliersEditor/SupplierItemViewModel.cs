#nullable enable
using System.ComponentModel.DataAnnotations;
using Wpf.Infrastructure;
using Wpf.Models;

namespace Wpf.ViewModels.SuppliersEditor;

// Представление элемента поставщика (поставщик в списке)
public class SupplierItemViewModel : ViewModelBase
{
    private string _name = null!;
    private bool _isSelected;

    // Идентификатор поставщика
    public int Id { get; }

    // Свойство для имени поставщика с валидацией
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

    // Свойство для проверки, выбран ли поставщик
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected)); // Уведомление об изменении состояния выбора
            }
        }
    }

    // Конструктор без параметров
    public SupplierItemViewModel()
    {
    }

    // Конструктор, инициализирующий модель данными поставщика
    public SupplierItemViewModel(Supplier supplier)
    {
        Id = supplier.Id;
        Name = supplier.Name;
        IsSelected = false; // По умолчанию поставщик не выбран
    }

    // Конструктор, создающий копию существующего SupplierItemViewModel
    public SupplierItemViewModel(SupplierItemViewModel supplierItemViewModel)
    {
        Id = supplierItemViewModel.Id;
        Name = supplierItemViewModel.Name;
        IsSelected = false; // По умолчанию поставщик не выбран
    }

    // Метод для валидации данных модели
    public ICollection<ValidationResult>? Validate()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();

        // Проверка на ошибки валидации
        if (Validator.TryValidateObject(this, context, results, true) is false)
        {
            return results; // Возвращаем список ошибок
        }

        return null; // Нет ошибок
    }
}
