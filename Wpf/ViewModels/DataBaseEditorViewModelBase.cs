#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

public abstract class DataBaseEditorViewModelBase : EditorViewModelBase
{
    private bool _isEditorPanelVisible;

    private Command _showAddItemPanelCmd = null!;
    private Command _showEditItemPanelCmd = null!;
    private Command _showDeleteItemPanelCmd = null!;
    private Command _refreshDataCmd = null!;

    public bool IsEditorPanelVisible
    {
        get => _isEditorPanelVisible;
        set
        {
            if (_isEditorPanelVisible != value)
            {
                _isEditorPanelVisible = value;
                RaisePropertyChanged(nameof(IsEditorPanelVisible));
                RaiseCommandCanExecuteChanged(_showAddItemPanelCmd);
                RaiseCommandCanExecuteChanged(_showEditItemPanelCmd);
                RaiseCommandCanExecuteChanged(_showDeleteItemPanelCmd);
                RaiseCommandCanExecuteChanged(_refreshDataCmd);
            }
        }
    }

    public Command ShowAddItemPanelCmd
    {
        get
        {
            return _showAddItemPanelCmd ??= new Command(ShowAddItemPanel, obj => CanExecuteShowAddItemPanelCmd(obj) && CanExecuteEditorCommands(obj));
        }
    }

    public Command ShowEditItemPanelCmd
    {
        get
        {
            return _showEditItemPanelCmd ??= new Command(ShowEditItemPanel, obj => CanExecuteShowEditItemPanelCmd(obj) && CanExecuteEditorCommands(obj));
        }
    }

    public Command ShowDeleteItemPanelCmd
    {
        get
        {
            return _showDeleteItemPanelCmd ??= new Command(ShowDeleteItemPanel, obj => CanExecuteShowDeleteItemPanelCmd(obj) && CanExecuteEditorCommands(obj));
        }
    }

    public Command RefreshDataCmd
    {
        get
        {
            return _refreshDataCmd ??= new Command(RefreshData, CanExecuteEditorCommands);
        }
    }

    protected DataBaseEditorViewModelBase(string name) : base(name)
    {

    }

    protected virtual void ShowAddItemPanel(object? obj)
    {
        IsEditorPanelVisible = true;
    }

    protected virtual bool CanExecuteShowAddItemPanelCmd(object? obj)
    {
        return true;
    }

    protected virtual void ShowEditItemPanel(object? obj)
    {
        IsEditorPanelVisible = true;
    }

    protected virtual bool CanExecuteShowEditItemPanelCmd(object? obj)
    {
        return true;
    }

    protected virtual void ShowDeleteItemPanel(object? obj)
    {

    }

    protected virtual bool CanExecuteShowDeleteItemPanelCmd(object? obj)
    {
        return true;
    }

    protected virtual void RefreshData(object? obj)
    {

    }

    protected virtual bool CanExecuteEditorCommands(object? obj)
    {
        return true;
    }
}
