using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.ViewModels.Configs.Senders;

// this class is needed because when sender config window is opened and its data context is set
// current DataContext view model must have 'CurrentSelectedConfigModelItem' member/propoerty
public class SenderConfigViewModelWrapper : INotifyPropertyChanged
{
    private SenderConfigViewModel _senderConfigViewModel;

    public SenderConfigViewModelWrapper(SenderConfigViewModel senderConfigViewModel)
    {
        _senderConfigViewModel = senderConfigViewModel;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public SenderConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _senderConfigViewModel;
        set
        {
            if (value == _senderConfigViewModel) return;
            _senderConfigViewModel = value;
            OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}