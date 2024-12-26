#nullable enable

// Используются пространства имен для доступа к необходимым сервисам и моделям.
using Wpf.Infrastructure;
using Wpf.Models;
using Wpf.Services.Interfaces;

namespace Wpf.ViewModels.MedicinesEditor;

// ViewModel для удаления медикамента
public class DeleteMedicineViewModel : EditorPanelViewModelBase<Medicine>
{
    private readonly IMedicineService _medicineService; // Сервис для работы с медикаментами
    private string _message = null!; // Сообщение для отображения пользователю

    // Элемент, представляющий удаляемый медикамент
    public MedicineItemViewModel DeletedMedicineItem { get; set; } = null!;

    // Сообщение, отображаемое в интерфейсе
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            RaisePropertyChanged(nameof(Message)); // Уведомление об изменении свойства
        }
    }

    // Конструктор, инициализирующий сервис медикаментов
    public DeleteMedicineViewModel(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    // Инициализация ViewModel данными медикамента, который нужно удалить
    public void Init(MedicineItemViewModel medicineItemViewModel)
    {
        DeletedMedicineItem = medicineItemViewModel; // Устанавливаем удаляемый медикамент
        Message = $"Удалить медикаменты {DeletedMedicineItem.Name}?"; // Формируем сообщение для пользователя
    }

    // Метод для сохранения изменений (удаления медикамента)
    protected override async void Save(object? obj)
    {
        try
        {
            // Асинхронно вызываем метод для удаления медикамента
            await _medicineService.DeleteMedicine(DeletedMedicineItem.Id);
            // Закрываем панель с результатом успеха
            ClosePanel(EditorPanelResult.Success, null);
        }
        catch (Exception ex)
        {
            // В случае ошибки показываем сообщение об ошибке
            ShowErrorMessage(ex.Message);
        }
    }
}
