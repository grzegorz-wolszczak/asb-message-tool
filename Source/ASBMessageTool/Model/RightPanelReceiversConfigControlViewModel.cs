using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.ReceivingMessages;

namespace ASBMessageTool.Model;

public class RightPanelReceiversConfigControlViewModel : INotifyPropertyChanged
{
    private readonly ReceiversConfigsViewModel _receiversConfigViewModel;

    public RightPanelReceiversConfigControlViewModel(ReceiversConfigsViewModel receiversConfigViewModel)
    {
        _receiversConfigViewModel = receiversConfigViewModel;
    }

    public IList<ReceiverConfigViewModel> ReceiverConfigViewModels => _receiversConfigViewModel.ReceiversConfigsVMs;

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
