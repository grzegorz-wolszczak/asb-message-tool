using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.ReceivingMessages.Code;

namespace ASBMessageTool.Model;

public class RightPanelReceiversConfigControlViewModel : INotifyPropertyChanged
{
    private readonly ReceiversConfigs _receiversConfig;

    public RightPanelReceiversConfigControlViewModel(ReceiversConfigs receiversConfig)
    {
        _receiversConfig = receiversConfig;
    }

    public IList<ReceiverConfigViewModel> ReceiverConfigViewModels => _receiversConfig.ReceiversConfigsVMs;

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
