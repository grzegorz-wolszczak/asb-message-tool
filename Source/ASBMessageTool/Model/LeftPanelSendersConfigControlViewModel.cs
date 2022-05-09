using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.SendingMessages;
using JetBrains.Annotations;

namespace ASBMessageTool.Model;

public sealed class LeftPanelSendersConfigControlViewModel : INotifyPropertyChanged
{
    private readonly SendersConfigs _sendersConfigs;

    public LeftPanelSendersConfigControlViewModel(SendersConfigs sendersConfigs)
    {
        _sendersConfigs = sendersConfigs;
        AddSenderConfigCommand = new DelegateCommand((_) => { sendersConfigs.AddNew(); });

        DeleteSenderConfigCommand = new DelegateCommand(_ => { sendersConfigs.Remove(CurrentSelectedConfigModelItem); },
            _ => CurrentSelectedConfigModelItem != null);

        MoveSenderConfigUpCommand = new DelegateCommand(_ => { sendersConfigs.MoveConfigUp(CurrentSelectedConfigModelItem);},
            _ => sendersConfigs.CanMoveUp(CurrentSelectedConfigModelItem));
        
        MoveSenderConfigDownCommand = new DelegateCommand(_ => { sendersConfigs.MoveConfigDown(CurrentSelectedConfigModelItem);},
            _ => sendersConfigs.CanMoveDown(CurrentSelectedConfigModelItem));
    }

    public ICommand AddSenderConfigCommand { get; }
    public ICommand DeleteSenderConfigCommand { get; }
    public ICommand MoveSenderConfigUpCommand { get; }
    public ICommand MoveSenderConfigDownCommand { get; }

    [UsedImplicitly]
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

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
