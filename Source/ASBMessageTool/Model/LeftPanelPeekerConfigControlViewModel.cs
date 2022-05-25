using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.PeekingMessages.Code;
using JetBrains.Annotations;

namespace ASBMessageTool.Model;

public class LeftPanelPeekerConfigControlViewModel : INotifyPropertyChanged
{
    private readonly PeekerConfigs _configs;

    public LeftPanelPeekerConfigControlViewModel(PeekerConfigs configs)
    {
        _configs = configs;
        AddPeekerConfigCommand = new DelegateCommand((_) =>
        {
            configs.AddNew();
        });

        DeletePeekerConfigCommand = new DelegateCommand(_ =>
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

    public ICommand AddPeekerConfigCommand { get; }
    public ICommand DeletePeekerConfigCommand { get; }
    
    public ICommand MoveConfigUpCommand { get; }
    public ICommand MoveConfigDownCommand { get; }


    [UsedImplicitly]
    public IList<PeekerConfigViewModel> PeekersConfigsVMs => _configs.PeekersConfigsVMs;

    public PeekerConfigViewModel CurrentSelectedConfigModelItem
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
