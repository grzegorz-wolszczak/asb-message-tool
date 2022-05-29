using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.SendingMessages.Code;
using JetBrains.Annotations;

namespace ASBMessageTool.Model;

public sealed class LeftPanelSendersConfigControlViewModel : INotifyPropertyChanged
{
    private readonly SendersConfigs _configs;

    public LeftPanelSendersConfigControlViewModel(SendersConfigs configs)
    {
        _configs = configs;
        AddSenderConfigCommand = new DelegateCommand((_) => { configs.AddNew(); });

        DeleteSenderConfigCommand = new DelegateCommand(_ =>
            {
                var question = "Are you sure you want to delete this item ?";
                if (UserInteractions.ShowYesNoQueryDialog(question, $"Config '{CurrentSelectedConfigModelItem.ModelItem.ConfigName}' deletion") == UserInteractions.YesNoDialogResult.Yes)
                {
                    configs.Remove(CurrentSelectedConfigModelItem);    
                }
            },
            _ => CurrentSelectedConfigModelItem != null);

        MoveConfigUpCommand = new DelegateCommand(_ => { configs.MoveConfigUp(CurrentSelectedConfigModelItem);},
            _ => configs.CanMoveUp(CurrentSelectedConfigModelItem));
        
        MoveConfigDownCommand = new DelegateCommand(_ => { configs.MoveConfigDown(CurrentSelectedConfigModelItem);},
            _ => configs.CanMoveDown(CurrentSelectedConfigModelItem));
    }

    public ICommand AddSenderConfigCommand { get; }
    public ICommand DeleteSenderConfigCommand { get; }
    public ICommand MoveConfigUpCommand { get; }
    public ICommand MoveConfigDownCommand { get; }

    [UsedImplicitly]
    public IList<SenderConfigViewModel> SendersConfigsVMs => _configs.SendersConfigsVMs;

    public SenderConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _configs.CurrentSelectedConfigModelItem;
        set
        {
            _configs.CurrentSelectedConfigModelItem = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
