using System;
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
        var messageSenderFactory = new MessageSenderFactory(_logger);

        var appDirectory = Directory.GetParent(Process.GetCurrentProcess().MainModule!.FileName!)!.ToString();
        var configFilePath = Path.Join(appDirectory, StaticConfig.ConfigFileName);

        var senderConfigViewModels = new ObservableCollection<SenderConfigViewModel>();

        SenderMessagePropertiesWindowProxyFactory senderMessagePropertiesWindowProxyFactory = new SenderMessagePropertiesWindowProxyFactory();
        var senderConfigModelFactory = new SenderConfigModelFactory();
        
        var senderConfigViewModelFactory = new SenderConfigViewModelFactory(_logger,
            inGuiThreadActionCaller,
            messageSenderFactory,
            senderMessagePropertiesWindowProxyFactory,
            new SenderSettingsValidator());
        var senderConfigWindowFactory = new SenderConfigWindowFactory();

        var sendersConfigViewModel = new SendersConfigs(
            senderConfigViewModels,
            senderConfigWindowFactory,
            senderConfigViewModelFactory,
            senderConfigModelFactory);

        var receiverConfigViewModels = new ObservableCollection<ReceiverConfigViewModel>();

        var receiversConfigsViewModelFactory = new ReceiversConfigsViewModelFactory();
        var receiversConfigViewModel = new ReceiversConfigsViewModel(
            receiverConfigViewModels,
            new ReceiverConfigWindowFactory(),
            _logger,
            new ReceivedMessageFormatter(_logger),
            new ServiceBusMessageReceiverFactory(_logger),
            new MessagePropertiesWindowProxyFactory(),
            new DeadLetterMessagePropertiesWindowProxyFactory(), receiversConfigsViewModelFactory);

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
        
        _persistentConfiguration.Load(); // Load() should called be AFTER _mainWindowShow() to setup some main window settings
        _mainWindow.Show();

        // owner must be set to object that was already shown
        _aboutWindow.Owner = _mainWindow;
    }

    public void Stop()
    {
        _persistentConfiguration.Save();
    }
}
