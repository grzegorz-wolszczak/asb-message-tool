using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Application.Persistence;
using ASBMessageTool.Gui;
using ASBMessageTool.Gui.Views;
using ASBMessageTool.Model;
using ASBMessageTool.ReceivingMessages;
using ASBMessageTool.SendingMessages;


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

        var senderConfigViewModels = new ObservableCollection<SenderConfigViewModel>();

        var senderConfigModelFactory = new SenderConfigModelFactory();
        
        var senderConfigViewModelFactory = new SenderConfigViewModelFactory(
            _logger,
            inGuiThreadActionCaller,
            new MessageSenderFactory(_logger, new SenderSettingsValidator()),
            new SenderMessagePropertiesWindowProxyFactory(),
            new SenderSettingsValidator());
        var senderConfigWindowFactory = new SenderConfigWindowFactory();

        var sendersConfigViewModel = new SendersConfigs(
            senderConfigViewModels,
            senderConfigWindowFactory,
            senderConfigViewModelFactory,
            senderConfigModelFactory);

        var receiverSettingsValidator = new ReceiverSettingsValidator(_logger);

        var receiverConfigViewModelFactory = new ReceiverConfigViewModelFactory(
            new ReceivedMessageFormatter(_logger), 
            new DeadLetterMessagePropertiesWindowProxyFactory(),
            new MessagePropertiesWindowProxyFactory(), 
            new ServiceBusMessageReceiverFactory(_logger, receiverSettingsValidator),
            receiverSettingsValidator,
            inGuiThreadActionCaller);
        
        var receiversConfigViewModel = new ReceiversConfigs(
            new ObservableCollection<ReceiverConfigViewModel>(),
            new ReceiverConfigWindowFactory(), 
            new ReceiversConfigModelFactory(), 
            receiverConfigViewModelFactory);

        var leftRightTabsSyncViewModel = new LeftRightTabsSyncViewModel();

        _mainWindow = new MainWindow(
            mainWindowViewModel,
            leftRightTabsSyncViewModel,
            new LeftPanelSendersConfigControlViewModel(sendersConfigViewModel),
            new RightPanelSendersConfigControlViewModel(sendersConfigViewModel),
            new LeftPanelReceiversConfigControlViewModel(receiversConfigViewModel),
            new RightPanelReceiversConfigControlViewModel(receiversConfigViewModel));

        var persistentOptions = new ApplicationPersistentOptions(mainWindowViewModel, 
            sendersConfigViewModel, 
            receiversConfigViewModel,
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
