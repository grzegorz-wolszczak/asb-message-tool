using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.SendingMessages;

namespace ASBMessageTool.Model;

public class RightPanelSendersConfigControlViewModel : INotifyPropertyChanged
{
    private readonly SendersConfigs _sendersConfigs;

    public RightPanelSendersConfigControlViewModel(SendersConfigs sendersConfigs)
    {
        _sendersConfigs = sendersConfigs;
    }

    public event PropertyChangedEventHandler PropertyChanged;


    public IList<SenderConfigViewModel> SenderConfigViewModels => _sendersConfigs.SendersConfigsVMs;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
