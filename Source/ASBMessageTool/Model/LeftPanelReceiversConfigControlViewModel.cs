using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.ReceivingMessages.Code;
using JetBrains.Annotations;

namespace ASBMessageTool.Model;

public class LeftPanelReceiversConfigControlViewModel : INotifyPropertyChanged
{
    private readonly ReceiversConfigs _configs;

    public LeftPanelReceiversConfigControlViewModel(ReceiversConfigs configs)
    {
        _configs = configs;
        AddReceiverConfigCommand = new DelegateCommand((_) =>
        {
            configs.AddNew();
        });

        DeleteReceiverConfigCommand = new DelegateCommand(_ =>
            {
                configs.Remove(CurrentSelectedConfigModelItem);
            },
            _ => CurrentSelectedConfigModelItem != null);
        
        MoveConfigUpCommand = new DelegateCommand(_ =>
            {
                configs.MoveConfigUp(CurrentSelectedConfigModelItem);
            },
            _ => configs.CanMoveUp(CurrentSelectedConfigModelItem));
        
        MoveConfigDownCommand = new DelegateCommand(_ =>
            {
                configs.MoveConfigDown(CurrentSelectedConfigModelItem);
            },
            _ => configs.CanMoveDown(CurrentSelectedConfigModelItem));
    }

    public ICommand AddReceiverConfigCommand { get; }
    public ICommand DeleteReceiverConfigCommand { get; }
    
    public ICommand MoveConfigUpCommand { get; }
    public ICommand MoveConfigDownCommand { get; }


    [UsedImplicitly]
    public IList<ReceiverConfigViewModel> ReceiversConfigsVMs => _configs.ReceiversConfigsVMs;

    public ReceiverConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _configs.CurrentSelectedConfigModelItem;
        set
        {
            _configs.CurrentSelectedConfigModelItem = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
