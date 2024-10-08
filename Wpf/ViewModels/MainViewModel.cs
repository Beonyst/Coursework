#nullable enable
using System.Collections.ObjectModel;
using Wpf.Infrastructure;
using Wpf.Models.Settings;
using Wpf.ViewModels.MedicinesEditor;
using Wpf.ViewModels.SuppliersEditor;

namespace Wpf.ViewModels;

public class MainViewModel : ViewModelBase
{
    private EditorViewModelBase? _selectedEditorViewModel;

    public ObservableCollection<EditorViewModelBase> EditorViewModels { get; }

    public EditorViewModelBase? SelectedEditorViewModel
    {
        get => _selectedEditorViewModel;
        set
        {
            if (_selectedEditorViewModel != value)
            {
                _selectedEditorViewModel = value;
                if (SelectedEditorViewModel is not null)
                {
                    _selectedEditorViewModel!.Update();
                }
                RaisePropertyChanged(nameof(SelectedEditorViewModel));
            }
        }
    }

    public MainViewModel(AppSettings appSettings, SuppliersEditorViewModel suppliersEditorViewModel, MedicinesEditorViewModel medicinesEditorViewModel)
    {
        EditorViewModels = new ObservableCollection<EditorViewModelBase>
        {
            suppliersEditorViewModel,
            medicinesEditorViewModel
        };

        foreach (var editor in EditorViewModels)
        {
            editor.ParentViewModel = this;
        }

        SelectedEditorViewModel = EditorViewModels.FirstOrDefault();
    }
}
