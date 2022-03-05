using System.Diagnostics;
using Main.Application.Logging;
using Main.Application.Persistence;
using Main.Commands;
using Main.ConfigsGuiMetadata;
using Main.ViewModels;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;
using Main.Windows;

namespace Main.Application;

public class ApplicationLogicRoot
{
   private readonly IApplicationProxy _appProxy;
   private MainWindow _mainWindow;
   private PersistentConfiguration _persistenConfig;
   private AboutWindow _aboutWindow;

   public ApplicationLogicRoot(IApplicationProxy appProxy)
   {
      _appProxy = appProxy;

      var binaryInfo = new ApplicationBinaryInfo(
         AppContants.ApplicationName,
         Process.GetCurrentProcess().MainModule.FileName);

      ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel = new ServiceBusSelectedConfigsViewModel();
      var senderConfigElementsGuiMetadataManager = new SenderConfigElementsGuiMetadataManager();

      _aboutWindow = new AboutWindow(new AboutWindowViewModel(binaryInfo));
      var aboutWindowProxy = new AboutWindowProxy(_aboutWindow);
      var mainViewModel = new MainViewModel(aboutWindowProxy);
      var logger = mainViewModel.GetLogger();


      var messageSenderFactory = new MessageSenderFactory(logger);

      var inGuiThreadActionCaller = new InGuiThreadActionCaller();
      SendersSelectedConfigViewModel sendersViewModel = new SendersSelectedConfigViewModel(
         senderConfigElementsGuiMetadataManager,
         inGuiThreadActionCaller,
         messageSenderFactory);
      var serviceBusMessageReceiverFactory = new ServiceBusMessageReceiverFactory(logger);
      ReceiversSelectedConfigViewModel receiversViewModel = new ReceiversSelectedConfigViewModel(serviceBusMessageReceiverFactory);

      var leftPanelControlViewModel = new LeftPanelControlViewModel(
         serviceBusConfigsViewModel,
         sendersViewModel,
         receiversViewModel);

      var persistentOptions = new ApplicationPersistentOptions(serviceBusConfigsViewModel, sendersViewModel, receiversViewModel);

      _persistenConfig = new PersistentConfiguration(binaryInfo, persistentOptions);

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
      return new MessageSender(_logger);
   }
}
