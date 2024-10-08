#nullable enable
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.MedicinesEditor;

public class DeleteMedicineViewModel : EditorPanelViewModelBase<Medicine>
{
    private readonly IMedicineService _medicineService;
    private string _message = null!;

    public MedicineItemViewModel DeletedMedicineItem { get; set; } = null!;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            RaisePropertyChanged(nameof(Message));
        }
    }

    public DeleteMedicineViewModel(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    public void Init(MedicineItemViewModel medicineItemViewModel)
    {
        DeletedMedicineItem = medicineItemViewModel;
        Message = $"Удалить медикаменты {DeletedMedicineItem.Name}?";
    }

    protected override async void Save(object? obj)
    {
        try
        {
            await _medicineService.DeleteMedicine(DeletedMedicineItem.Id);
            ClosePanel(EditorPanelResult.Success, null);
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }
}
