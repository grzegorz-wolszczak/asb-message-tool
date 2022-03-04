using System.Windows;
using System.Windows.Controls;
using Main.ViewModels;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;

namespace Main.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   public MainWindow(
      MainViewModel mainViewModel,
      ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel,
      SendersSelectedConfigViewModel sendersViewModel,
      ReceiversSelectedConfigViewModel receiversViewModel,
      LeftPanelControlViewModel leftPanelControlViewModel)
   {
      InitializeComponent();
      DataContext = mainViewModel;
      leftPanelControl.DataContext = leftPanelControlViewModel;
      rightPanelControl.DataContext = sendersViewModel;
      leftPanelControl.leftPanelServiceBusConfigTabItem.DataContext = leftPanelControlViewModel;
      leftPanelControl.leftPanelSendersConfigTabItem.DataContext = leftPanelControlViewModel;
      leftPanelControl.leftPanelReceiversConfigTabItem.DataContext = leftPanelControlViewModel;
      leftPanelControl.leftPanelServiceBusConfigUserControl.DataContext = serviceBusConfigsViewModel;
      rightPanelControl.rightPanelServiceBusConfigTab.DataContext = serviceBusConfigsViewModel;
      leftPanelControl.leftPanelSendersConfigUserControl.DataContext = sendersViewModel;
      rightPanelControl.rightPanelSenderConfigTab.DataContext = sendersViewModel;
      leftPanelControl.leftPanelReceiversConfigUserControl.DataContext = receiversViewModel;
      rightPanelControl.rightPanelReceiverConfigTab.DataContext = receiversViewModel;
   }

   private void LogContentTextBoxChanged(object sender, TextChangedEventArgs e)
   {
      /*
       * <x:Code>
                    <![CDATA[
                           void LogContentTextBoxChanged(object sender, RoutedEventArgs e)
                           {
                              if(((ViewModels.MainViewModel)this.DataContext).ShouldScrollToEndOnLogContentChange)
                              {
                                this.logContentTextBox.ScrollToEnd();
                              }

                           }
                        ]]>
                </x:Code>
       */
   }
}
