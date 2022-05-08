using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.SendingMessages;

namespace ASBMessageTool.Model;

public class LeftPanelSendersConfigControlViewModel : INotifyPropertyChanged
{
    private readonly SendersConfigs _sendersConfigs;

    public LeftPanelSendersConfigControlViewModel(SendersConfigs sendersConfigs)
    {
        _sendersConfigs = sendersConfigs;
        AddSenderConfigCommand = new DelegateCommand((_) =>
        {
            sendersConfigs.AddNew();
        });

        DeleteSenderConfigCommand = new DelegateCommand(_ =>
            {
                sendersConfigs.Remove(CurrentSelectedConfigModelItem);
            },
            _ => CurrentSelectedConfigModelItem != null);
    }

    public ICommand AddSenderConfigCommand { get; }
    public ICommand DeleteSenderConfigCommand { get; }


    public IList<SenderConfigViewModel> SendersConfigsVMs => _sendersConfigs.SendersConfigsVMs;

    public SenderConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _sendersConfigs.CurrentSelectedConfigModelItem;
        set
        {
            _sendersConfigs.CurrentSelectedConfigModelItem = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
