using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.ReceivingMessages;

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
    }

    public ICommand AddReceiverConfigCommand { get; }
    public ICommand DeleteReceiverConfigCommand { get; }


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
