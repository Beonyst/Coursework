#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

// Абстрактный класс DataBaseEditorViewModelBase расширяет EditorViewModelBase и добавляет функциональность для управления панелями добавления, редактирования, удаления и обновления данных.
public abstract class DataBaseEditorViewModelBase : EditorViewModelBase
{
    private bool _isEditorPanelVisible;

    private Command _showAddItemPanelCmd = null!;
    private Command _showEditItemPanelCmd = null!;
    private Command _showDeleteItemPanelCmd = null!;
    private Command _refreshDataCmd = null!;

    // Свойство, определяющее видимость панели редактора
    public bool IsEditorPanelVisible
    {
        get => _isEditorPanelVisible;
        set
        {
            if (_isEditorPanelVisible != value) // Проверка на изменение видимости панели
            {
                _isEditorPanelVisible = value;
                RaisePropertyChanged(nameof(IsEditorPanelVisible)); // Уведомление об изменении видимости
                RaiseCommandCanExecuteChanged(_showAddItemPanelCmd); // Обновление состояния команд
                RaiseCommandCanExecuteChanged(_showEditItemPanelCmd); // Обновление состояния команд
                RaiseCommandCanExecuteChanged(_showDeleteItemPanelCmd); // Обновление состояния команд
                RaiseCommandCanExecuteChanged(_refreshDataCmd); // Обновление состояния команд
            }
        }
    }

    // Команда для отображения панели добавления элемента
    public Command ShowAddItemPanelCmd
    {
        get
        {
            return _showAddItemPanelCmd ??= new Command(ShowAddItemPanel, obj => CanExecuteShowAddItemPanelCmd(obj) && CanExecuteEditorCommands(obj));
        }
    }

    // Команда для отображения панели редактирования элемента
    public Command ShowEditItemPanelCmd
    {
        get
        {
            return _showEditItemPanelCmd ??= new Command(ShowEditItemPanel, obj => CanExecuteShowEditItemPanelCmd(obj) && CanExecuteEditorCommands(obj));
        }
    }

    // Команда для отображения панели удаления элемента
    public Command ShowDeleteItemPanelCmd
    {
        get
        {
            return _showDeleteItemPanelCmd ??= new Command(ShowDeleteItemPanel, obj => CanExecuteShowDeleteItemPanelCmd(obj) && CanExecuteEditorCommands(obj));
        }
    }

    // Команда для обновления данных
    public Command RefreshDataCmd
    {
        get
        {
            return _refreshDataCmd ??= new Command(RefreshData, CanExecuteEditorCommands);
        }
    }

    // Конструктор, который инициализирует имя редактора
    protected DataBaseEditorViewModelBase(string name) : base(name)
    {

    }

    // Метод для отображения панели добавления элемента
    protected virtual void ShowAddItemPanel(object? obj)
    {
        IsEditorPanelVisible = true;
    }

    // Метод для проверки возможности выполнения команды добавления элемента
    protected virtual bool CanExecuteShowAddItemPanelCmd(object? obj)
    {
        return true;
    }

    // Метод для отображения панели редактирования элемента
    protected virtual void ShowEditItemPanel(object? obj)
    {
        IsEditorPanelVisible = true;
    }

    // Метод для проверки возможности выполнения команды редактирования элемента
    protected virtual bool CanExecuteShowEditItemPanelCmd(object? obj)
    {
        return true;
    }

    // Метод для отображения панели удаления элемента (пока не реализован)
    protected virtual void ShowDeleteItemPanel(object? obj)
    {

    }

    // Метод для проверки возможности выполнения команды удаления элемента
    protected virtual bool CanExecuteShowDeleteItemPanelCmd(object? obj)
    {
        return true;
    }

    // Метод для обновления данных (пока не реализован)
    protected virtual void RefreshData(object? obj)
    {

    }

    // Метод для проверки возможности выполнения команд редактора
    protected virtual bool CanExecuteEditorCommands(object? obj)
    {
        return true;
    }
}
