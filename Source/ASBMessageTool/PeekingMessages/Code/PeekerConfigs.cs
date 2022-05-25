using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekerConfigs : INotifyPropertyChanged
{
    private readonly ObservableCollection<PeekerConfigViewModel> _peekerConfigViewModels;
    private readonly PeekerConfigWindowFactory _peekerConfigWindowFactory;
    private readonly PeekerConfigModelFactory _peekerConfigModelFactory;
    private readonly PeekerConfigViewModelFactory _peekerConfigViewModelFactory;
    private readonly Dictionary<PeekerConfigViewModel, PeekerConfigStandaloneWindowViewer> _windowsForItems = new();
    private PeekerConfigViewModel _currentSelectedItem;

    public PeekerConfigs(
        ObservableCollection<PeekerConfigViewModel> peekerConfigViewModels, 
        PeekerConfigWindowFactory peekerConfigWindowFactory,
        PeekerConfigModelFactory peekerConfigModelFactory, 
        PeekerConfigViewModelFactory peekerConfigViewModelFactory)
    {
        _peekerConfigViewModels = peekerConfigViewModels;
        _peekerConfigWindowFactory = peekerConfigWindowFactory;
        _peekerConfigModelFactory = peekerConfigModelFactory;
        _peekerConfigViewModelFactory = peekerConfigViewModelFactory;
    }

    public void AddNewForModelItem(PeekerConfigModel item)
    {
        
        var windowForConfig = _peekerConfigWindowFactory.CreateWindowForConfig();
        var viewModel = _peekerConfigViewModelFactory.Create(item, windowForConfig);
        
        windowForConfig.SetDataContext(viewModel);
        PeekersConfigsVMs.Add(viewModel);
        _windowsForItems.Add(viewModel, windowForConfig);
    }
    
    
    
    public PeekerConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _currentSelectedItem;
        set
        {
            if (value == _currentSelectedItem) return;
            _currentSelectedItem = value;
            OnPropertyChanged();
        }
    }

    
    public IList<PeekerConfigViewModel> PeekersConfigsVMs => _peekerConfigViewModels;
    
    public void AddNew()
    {
        var item = _peekerConfigModelFactory.Create();
        AddNewForModelItem(item);
    }

    public void Remove(PeekerConfigViewModel item)
    {
        _windowsForItems[item].DeleteWindow();
        _peekerConfigViewModels.Remove(item);
        _windowsForItems.Remove(item);
    }

    public void MoveConfigUp(PeekerConfigViewModel item)
    {
        if (!CanMoveUp(item)) return;
        var oldIndex = _peekerConfigViewModels.IndexOf(item);
        _peekerConfigViewModels.Move(oldIndex, oldIndex-1);
    }

    public void MoveConfigDown(PeekerConfigViewModel item)
    {
        if (!CanMoveDown(item)) return;
        var oldIndex = _peekerConfigViewModels.IndexOf(item);
        _peekerConfigViewModels.Move(oldIndex, oldIndex+1);
    }

    public bool CanMoveDown(PeekerConfigViewModel item)
    {
        if (item is null) return false;
        return _peekerConfigViewModels.IndexOf(item) < _peekerConfigViewModels.Count-1;
    }

    public bool CanMoveUp(PeekerConfigViewModel item)
    {
        if (item is null) return false;
        return _peekerConfigViewModels.IndexOf(item) > 0;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
