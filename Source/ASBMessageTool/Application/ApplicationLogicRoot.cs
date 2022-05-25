using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Application.Persistence;
using ASBMessageTool.Gui;
using ASBMessageTool.Gui.Views;
using ASBMessageTool.Model;
using ASBMessageTool.PeekingMessages.Code;
using ASBMessageTool.ReceivingMessages.Code;
using ASBMessageTool.SendingMessages.Code;


namespace ASBMessageTool.Application;

public class ApplicationLogicRoot
{
    private readonly MainWindow _mainWindow;
    private readonly AboutWindow _aboutWindow;
    private PersistentConfiguration _persistentConfiguration;
    private IServiceBusHelperLogger _logger;

    public ApplicationLogicRoot(ApplicationShutdowner applicationShutdowner)
    {
        var inGuiThreadActionCaller = new InGuiThreadActionCaller();
        _aboutWindow = new AboutWindow();
        var aboutWindowProxy = new AboutWindowProxy(_aboutWindow);

        var mainWindowViewModel = new MainViewModel(aboutWindowProxy);
        _logger = mainWindowViewModel.GetLogger();

        var appDirectory = Directory.GetParent(Process.GetCurrentProcess().MainModule!.FileName!)!.ToString();
        var configFilePath = Path.Join(appDirectory, StaticConfig.ConfigFileName);


        var operationSystemServices = new OperationSystemServices();


        // senders
        var senderConfigViewModels = new ObservableCollection<SenderConfigViewModel>();
        var senderConfigModelFactory = new SenderConfigModelFactory();
        var senderConfigViewModelFactory = new SenderConfigViewModelFactory(
            _logger,
            inGuiThreadActionCaller,
            new MessageSenderFactory(_logger, new SenderSettingsValidator()),
            new SenderMessagePropertiesWindowProxyFactory(),
            new SenderSettingsValidator(),
            operationSystemServices);
        var senderConfigWindowFactory = new SenderConfigWindowFactory();

        var sendersConfigViewModel = new SendersConfigs(
            senderConfigViewModels,
            senderConfigWindowFactory,
            senderConfigViewModelFactory,
            senderConfigModelFactory);

        
        // receivers
        var receiverSettingsValidator = new ReceiverSettingsValidator(_logger);
        
        var receiverConfigViewModelFactory = new ReceiverConfigViewModelFactory(
            new ReceivedMessageFormatter(_logger), 
            new DeadLetterMessagePropertiesWindowProxyFactory(),
            new MessagePropertiesWindowProxyFactory(), 
            new ServiceBusMessageReceiverFactory(_logger, receiverSettingsValidator),
            receiverSettingsValidator,
            inGuiThreadActionCaller, 
            operationSystemServices);
        
        var receiversConfigViewModel = new ReceiversConfigs(
            new ObservableCollection<ReceiverConfigViewModel>(),
            new ReceiverConfigWindowFactory(), 
            new ReceiversConfigModelFactory(), 
            receiverConfigViewModelFactory);

        // peekers
        var peekerSettingsValidator = new PeekerSettingsValidator(_logger);

        var peekerConfigViewModelFactory = new PeekerConfigViewModelFactory(
            new ReceivedMessageFormatter(_logger),
            new ServiceBusMessagePeekerFactory(_logger, peekerSettingsValidator),
            peekerSettingsValidator,
            inGuiThreadActionCaller,
            operationSystemServices);

        var peekersConfigViewModel = new PeekerConfigs(
            new ObservableCollection<PeekerConfigViewModel>(),
            new PeekerConfigWindowFactory(),
            new PeekerConfigModelFactory(),
            peekerConfigViewModelFactory);
        
        
        var leftRightTabsSyncViewModel = new LeftRightTabsSyncViewModel();

        _mainWindow = new MainWindow(
            mainWindowViewModel,
            leftRightTabsSyncViewModel,
            new LeftPanelSendersConfigControlViewModel(sendersConfigViewModel),
            new RightPanelSendersConfigControlViewModel(sendersConfigViewModel),
            new LeftPanelReceiversConfigControlViewModel(receiversConfigViewModel),
            new RightPanelReceiversConfigControlViewModel(receiversConfigViewModel),
            new LeftPanelPeekerConfigControlViewModel(peekersConfigViewModel),
            new RightPanelPeekerConfigControlViewModel(peekersConfigViewModel));

        var persistentOptions = new ApplicationPersistentOptions(
            mainWindowViewModel, 
            sendersConfigViewModel, 
            receiversConfigViewModel,
            peekersConfigViewModel,
            leftRightTabsSyncViewModel);
        
        _persistentConfiguration = new PersistentConfiguration(configFilePath, persistentOptions, _logger, applicationShutdowner);
    }

    public void Start()
    {
        _mainWindow.InitializeComponent();
        System.Windows.Application.Current.MainWindow = _mainWindow;

        _mainWindow.Show();
        _persistentConfiguration.Load(); // Load() should called be AFTER _mainWindowShow() to setup some main window settings

        // owner must be set to object that was already shown
        _aboutWindow.Owner = _mainWindow;
    }

    public void Stop()
    {
        _persistentConfiguration.Save();
    }
}
