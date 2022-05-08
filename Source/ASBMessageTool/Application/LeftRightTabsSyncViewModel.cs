using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ASBMessageTool.Application;

public class LeftRightTabsSyncViewModel : INotifyPropertyChanged
{
    private bool _isLeftPanelSendersTabSelected;
    private bool _isLeftPanelReceiversTabSelected;
    private int _selectedLeftPanelTabIndex;

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
