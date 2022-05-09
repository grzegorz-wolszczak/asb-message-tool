using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.ReceivingMessages;
using JetBrains.Annotations;

namespace ASBMessageTool.Model;

public class LeftPanelReceiversConfigControlViewModel : INotifyPropertyChanged
{
    private readonly ReceiversConfigsViewModel _configsViewModel;

    public LeftPanelReceiversConfigControlViewModel(ReceiversConfigsViewModel configsViewModel)
    {
        _configsViewModel = configsViewModel;
        AddReceiverConfigCommand = new DelegateCommand((_) =>
        {
            configsViewModel.AddNew();
        });

        DeleteReceiverConfigCommand = new DelegateCommand(_ =>
            {
                configsViewModel.Remove(CurrentSelectedConfigModelItem);
            },
            _ => CurrentSelectedConfigModelItem != null);
        
        MoveReceiverConfigUpCommand = new DelegateCommand(_ =>
            {
                configsViewModel.MoveConfigUp(CurrentSelectedConfigModelItem);
            },
            _ => configsViewModel.CanMoveUp(CurrentSelectedConfigModelItem));
        
        MoveReceiverConfigDownCommand = new DelegateCommand(_ =>
            {
                configsViewModel.MoveConfigDown(CurrentSelectedConfigModelItem);
            },
            _ => configsViewModel.CanMoveDown(CurrentSelectedConfigModelItem));
    }

    public ICommand AddReceiverConfigCommand { get; }
    public ICommand DeleteReceiverConfigCommand { get; }
    
    public ICommand MoveReceiverConfigUpCommand { get; }
    public ICommand MoveReceiverConfigDownCommand { get; }


    [UsedImplicitly]
    public IList<ReceiverConfigViewModel> ReceiversConfigsVMs => _configsViewModel.ReceiversConfigsVMs;

    public ReceiverConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _configsViewModel.CurrentSelectedConfigModelItem;
        set
        {
            _configsViewModel.CurrentSelectedConfigModelItem = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
