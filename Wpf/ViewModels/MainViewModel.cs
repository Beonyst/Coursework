#nullable enable
using System.Collections.ObjectModel;
using Wpf.Infrastructure;
using Wpf.Models.Settings;
using Wpf.ViewModels.MedicinesEditor;
using Wpf.ViewModels.SuppliersEditor;

namespace Wpf.ViewModels;

// MainViewModel управляет выбором и обновлением редакторов для поставщиков и медикаментов.
public class MainViewModel : ViewModelBase
{
    private EditorViewModelBase? _selectedEditorViewModel;

    // Коллекция редакторов, доступных в приложении
    public ObservableCollection<EditorViewModelBase> EditorViewModels { get; }

    // Свойство, которое хранит выбранный редактор и обновляет его, если оно меняется
    public EditorViewModelBase? SelectedEditorViewModel
    {
        get => _selectedEditorViewModel;
        set
        {
            if (_selectedEditorViewModel != value) // Проверка на изменения
            {
                _selectedEditorViewModel = value;
                if (SelectedEditorViewModel is not null) // Если новый редактор выбран, обновляем его
                {
                    _selectedEditorViewModel!.Update();
                }
                RaisePropertyChanged(nameof(SelectedEditorViewModel)); // Уведомляем об изменении
            }
        }
    }

    // Конструктор класса, который инициализирует коллекцию редакторов и устанавливает начальный выбранный редактор
    public MainViewModel(AppSettings appSettings, SuppliersEditorViewModel suppliersEditorViewModel, MedicinesEditorViewModel medicinesEditorViewModel)
    {
        // Инициализация коллекции редакторов
        EditorViewModels = new ObservableCollection<EditorViewModelBase>
        {
            suppliersEditorViewModel,
            medicinesEditorViewModel
        };

        // Установка родительского ViewModel для каждого редактора
        foreach (var editor in EditorViewModels)
        {
            editor.ParentViewModel = this;
        }

        // Установка первого редактора как выбранного
        SelectedEditorViewModel = EditorViewModels.FirstOrDefault();
    }
}
