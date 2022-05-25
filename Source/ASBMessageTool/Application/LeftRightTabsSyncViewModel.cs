using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ASBMessageTool.Application;

public class LeftRightTabsSyncViewModel : INotifyPropertyChanged
{
    private bool _isLeftPanelSendersTabSelected;
    private bool _isLeftPanelReceiversTabSelected;
    private bool _isLeftPanelPeekersTabSelected;
    private int _selectedLeftPanelTabIndex;

    [UsedImplicitly]
    public bool IsLeftPanelSendersTabSelected
    {
        get => _isLeftPanelSendersTabSelected;
        set
        {
            if (value == _isLeftPanelSendersTabSelected) return;
            _isLeftPanelSendersTabSelected = value;
            OnPropertyChanged();
        }
    }

    public int SelectedLeftPanelTabIndex
    {
        get => _selectedLeftPanelTabIndex;
        set
        {
            if (value == _selectedLeftPanelTabIndex) return;
            _selectedLeftPanelTabIndex = value;
            OnPropertyChanged();
        }
    }


    [UsedImplicitly]
    public bool IsLeftPanelPeekersTabSelected
    {
        get => _isLeftPanelPeekersTabSelected;
        set
        {
            if (value == _isLeftPanelPeekersTabSelected) return;
            _isLeftPanelPeekersTabSelected = value;
            OnPropertyChanged();
        }
    }
    
    [UsedImplicitly]
    public bool IsLeftPanelReceiversTabSelected
    {
        get => _isLeftPanelReceiversTabSelected;
        set
        {
            if (value == _isLeftPanelReceiversTabSelected) return;
            _isLeftPanelReceiversTabSelected = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
