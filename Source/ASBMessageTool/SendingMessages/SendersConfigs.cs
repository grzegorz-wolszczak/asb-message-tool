using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.SendingMessages.Gui;

namespace ASBMessageTool.SendingMessages;



public sealed class SendersConfigs : INotifyPropertyChanged
{
    private SenderConfigViewModel _currentSelectedItem;
    private readonly ObservableCollection<SenderConfigViewModel> _senderConfigViewModels;
    private readonly Dictionary<SenderConfigViewModel, SenderConfigStandaloneWindowViewer> _windowsForItems = new();
    private readonly ISenderConfigWindowFactory _senderConfigWindowFactory;
    private readonly SenderConfigViewModelFactory _senderConfigViewModelFactory;
    private readonly SenderConfigModelFactory _senderConfigModelFactory;

    public SendersConfigs(
        ObservableCollection<SenderConfigViewModel> senderConfigViewModels,
        ISenderConfigWindowFactory senderConfigWindowFactory,
        SenderConfigViewModelFactory senderConfigViewModelFactory, SenderConfigModelFactory senderConfigModelFactory)
    {
        _senderConfigViewModels = senderConfigViewModels;
        _senderConfigWindowFactory = senderConfigWindowFactory;
        _senderConfigViewModelFactory = senderConfigViewModelFactory;
        _senderConfigModelFactory = senderConfigModelFactory;
    }

    public void AddNewForModelItem(SenderConfigModel senderConfigModel)
    {
        var windowForSenderConfig = _senderConfigWindowFactory.CreateWindowForConfig();

        var viewModel = _senderConfigViewModelFactory.Create(senderConfigModel, windowForSenderConfig);

        windowForSenderConfig.SetDataContext(viewModel);

        SendersConfigsVMs.Add(viewModel);
        _windowsForItems.Add(viewModel, windowForSenderConfig);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public IList<SenderConfigViewModel> SendersConfigsVMs => _senderConfigViewModels;

    public SenderConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _currentSelectedItem;
        set
        {
            if (value == _currentSelectedItem) return;
            _currentSelectedItem = value;
            OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void AddNew()
    {
        var senderConfigModel = _senderConfigModelFactory.Create();

        AddNewForModelItem(senderConfigModel);
    }

    public void Remove(SenderConfigViewModel item)
    {
        _windowsForItems[item].DeleteWindow();
        _senderConfigViewModels.Remove(item);
        _windowsForItems.Remove(item);
    }
}
