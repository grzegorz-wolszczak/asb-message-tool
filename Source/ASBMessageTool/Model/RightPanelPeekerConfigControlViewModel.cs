using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.PeekingMessages.Code;

namespace ASBMessageTool.Model;

public class RightPanelPeekerConfigControlViewModel: INotifyPropertyChanged
{
    private readonly PeekerConfigs _peekersConfigViewModel;

    public RightPanelPeekerConfigControlViewModel(PeekerConfigs peekersConfigViewModel)
    {
        _peekersConfigViewModel = peekersConfigViewModel;
    }
    
    public IList<PeekerConfigViewModel> PeekerConfigViewModels => _peekersConfigViewModel.PeekersConfigsVMs;
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
