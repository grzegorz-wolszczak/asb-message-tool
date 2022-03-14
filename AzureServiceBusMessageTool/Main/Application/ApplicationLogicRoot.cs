using System.Diagnostics;
using Main.Application.Logging;
using Main.Application.Persistence;
using Main.Commands;
using Main.ConfigsGuiMetadata;
using Main.Validations;
using Main.ViewModels;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;
using Main.ViewModels.Configs.Senders.MessagePropertyWindow;
using Main.Windows;
using Main.Windows.MainWindow;

namespace Main.Application;

public class ApplicationLogicRoot
{
    private readonly IApplicationProxy _appProxy;
    private MainWindow _mainWindow;
    private PersistentConfiguration _persistenConfig;
    private AboutWindow _aboutWindow;
    private IServiceBusHelperLogger _logger;

    public ApplicationLogicRoot(IApplicationProxy appProxy)
    {
        _appProxy = appProxy;

        var binaryInfo = new ApplicationBinaryInfo(
            StaticConfig.ApplicationName,
            Process.GetCurrentProcess().MainModule.FileName);

        ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel = new ServiceBusSelectedConfigsViewModel();
        var senderConfigElementsGuiMetadataManager = new SenderConfigElementsGuiMetadataManager();

        _aboutWindow = new AboutWindow(new AboutWindowViewModel(binaryInfo));
        var aboutWindowProxy = new AboutWindowProxy(_aboutWindow);
        var mainViewModel = new MainViewModel(aboutWindowProxy);
        _logger = mainViewModel.GetLogger();


        var messageSenderFactory = new MessageSenderFactory(_logger);

        var inGuiThreadActionCaller = new InGuiThreadActionCaller();
        var messagePropertiesWindowFactory = new SenderMessagePropertiesWindowProxyFactory();
        SendersSelectedConfigViewModel sendersViewModel = new SendersSelectedConfigViewModel(
            senderConfigElementsGuiMetadataManager,
            inGuiThreadActionCaller,
            messageSenderFactory,
            messagePropertiesWindowFactory,
            _logger
        );
        var serviceBusMessageReceiverFactory = new ServiceBusMessageReceiverFactory(_logger);
        var messageApplicationPropertiesFactory = new MessagePropertiesWindowProxyFactory();
        var deadLetterMessagePropertiesWindowProxyFactory = new DeadLetterMessagePropertiesWindowProxyFactory();
        ReceiversSelectedConfigViewModel receiversViewModel = new ReceiversSelectedConfigViewModel(
            serviceBusMessageReceiverFactory,
            messageApplicationPropertiesFactory,
            deadLetterMessagePropertiesWindowProxyFactory
            );

        var leftPanelControlViewModel = new LeftPanelControlViewModel(
            serviceBusConfigsViewModel,
            sendersViewModel,
            receiversViewModel);

        var persistentOptions = new ApplicationPersistentOptions(mainViewModel,
            serviceBusConfigsViewModel,
            sendersViewModel,
            receiversViewModel);

        _persistenConfig = new PersistentConfiguration(_logger,binaryInfo,
            persistentOptions);

        _mainWindow = new MainWindow(mainViewModel,
            serviceBusConfigsViewModel,
            sendersViewModel,
            receiversViewModel,
            leftPanelControlViewModel);
    }

    public void Start()
    {
        _mainWindow.InitializeComponent();
        _persistenConfig.Load();
        _mainWindow.Show();

        // below line is needed, without that , after closing main window process still exists;
        _aboutWindow.Owner = _mainWindow;
    }

    public void Stop()
    {
        _persistenConfig.Save();
    }
}

public class MessageSenderFactory
{
    private readonly IServiceBusHelperLogger _logger;

    public MessageSenderFactory(IServiceBusHelperLogger logger)
    {
        _logger = logger;
    }

    public IMessageSender Create()
    {
        var validator = new SenderSettingsValidator();
        return new MessageSender(validator,_logger);
    }
}
