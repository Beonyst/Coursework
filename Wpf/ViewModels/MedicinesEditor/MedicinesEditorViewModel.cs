#nullable enable
using System.Collections.ObjectModel;
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.MedicinesEditor;

public class MedicinesEditorViewModel : DataBaseEditorViewModelBase
{
    private readonly IMedicineService _medicineService;
    private readonly ISupplierService _supplierService;
    private readonly AddMedicineViewModel _addMedicineViewModel;
    private readonly EditMedicineViewModel _editMedicineViewModel;
    private ObservableCollection<MedicineItemViewModel> _medicineItems = null!;
    private MedicineItemViewModel? _selectedMedicineItem;
    private EditorPanelViewModelBase<Medicine> _editorPanelViewModel;
    private bool _isDeletePanelVisible;

    public ObservableCollection<MedicineItemViewModel> MedicineItems
    {
        get => _medicineItems;
        set
        {
            if (_medicineItems != value)
            {
                _medicineItems = value;
                RaisePropertyChanged(nameof(MedicineItems));
            }
        }
    }

    public MedicineItemViewModel? SelectedMedicineItem
    {
        get => _selectedMedicineItem;
        set
        {
            if (_selectedMedicineItem != value)
            {
                _selectedMedicineItem = value;
                RaisePropertyChanged(nameof(SelectedMedicineItem));
                RaiseCommandCanExecuteChanged(ShowEditItemPanelCmd);
                RaiseCommandCanExecuteChanged(ShowDeleteItemPanelCmd);
            }
        }
    }

    public EditorPanelViewModelBase<Medicine> EditorPanelViewModel
    {
        get => _editorPanelViewModel;
        set
        {
            if (_editorPanelViewModel != value)
            {
                _editorPanelViewModel = value;
                RaisePropertyChanged(nameof(EditorPanelViewModel));
            }
        }
    }

    public DeleteMedicineViewModel DeleteMedicineViewModel { get; set; }

    public bool IsDeletePanelVisible
    {
        get => _isDeletePanelVisible;
        set
        {
            if (_isDeletePanelVisible != value)
            {
                _isDeletePanelVisible = value;
                RaisePropertyChanged(nameof(IsDeletePanelVisible));
            }
        }
    }

    public MedicinesEditorViewModel(IMedicineService medicineService, ISupplierService supplierService, DeleteMedicineViewModel deleteMedicineViewModel,
        AddMedicineViewModel addMedicineViewModel, EditMedicineViewModel editMedicineViewModel) : base("Медикаменты")
    {
        _medicineService = medicineService;
        _supplierService = supplierService;

        _addMedicineViewModel = addMedicineViewModel;
        _addMedicineViewModel.ParentViewModel = this;
        _addMedicineViewModel.EditorPanelClosed += AddMedicineViewModelOnEditorPanelClosed;

        _editMedicineViewModel = editMedicineViewModel;
        _editMedicineViewModel.EditorPanelClosed += EditMedicineViewModelOnEditorPanelClosed;

        DeleteMedicineViewModel = deleteMedicineViewModel;
        DeleteMedicineViewModel.EditorPanelClosed += DeleteMedicineViewModelOnEditorPanelClosed;
    }

    public override async void Update()
    {
        var medicines = await _medicineService.GetAllAsync();
        MedicineItems = new ObservableCollection<MedicineItemViewModel>(medicines.Select(medicine => new MedicineItemViewModel(medicine)));
    }

    protected override void RefreshData(object? obj)
    {
        Update();
    }

    protected override void ShowAddItemPanel(object? obj)
    {
        _addMedicineViewModel.Init();
        EditorPanelViewModel = _addMedicineViewModel;
        base.ShowAddItemPanel(obj);
    }

    protected override void ShowDeleteItemPanel(object? obj)
    {
        DeleteMedicineViewModel.Init(SelectedMedicineItem);
        IsDeletePanelVisible = true;
    }

    protected override bool CanExecuteShowDeleteItemPanelCmd(object? obj)
    {
        return SelectedMedicineItem is not null;
    }

    protected override void ShowEditItemPanel(object? obj)
    {
        _editMedicineViewModel.Init(SelectedMedicineItem!);
        EditorPanelViewModel = _editMedicineViewModel;
        base.ShowEditItemPanel(obj);
    }

    protected override bool CanExecuteShowEditItemPanelCmd(object? obj)
    {
        return SelectedMedicineItem is not null;
    }

    protected override bool CanExecuteEditorCommands(object? obj)
    {
        return IsEditorPanelVisible is false && IsDeletePanelVisible is false;
    }

    private void AddMedicineViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Medicine> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            MedicineItems.Add(new MedicineItemViewModel(editorPanelClosedEventArgs.EditedModel));
        }

        IsEditorPanelVisible = false;
    }

    private void EditMedicineViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Medicine> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            var medicineToReplace = MedicineItems.FirstOrDefault(medicine => medicine.Id == editorPanelClosedEventArgs.EditedModel.Id);
            if (medicineToReplace is not null)
            {
                var index = MedicineItems.IndexOf(medicineToReplace);
                MedicineItems.RemoveAt(index);
                MedicineItems.Insert(index, new MedicineItemViewModel(editorPanelClosedEventArgs.EditedModel));
            }
        }

        IsEditorPanelVisible = false;
    }

    private void DeleteMedicineViewModelOnEditorPanelClosed(object? sender, EditorPanelClosedEventArgs<Medicine> editorPanelClosedEventArgs)
    {
        if (editorPanelClosedEventArgs.ResultType == EditorPanelResult.Success)
        {
            MedicineItems.Remove(DeleteMedicineViewModel.DeletedMedicineItem);
        }

        IsDeletePanelVisible = false;
    }
}
