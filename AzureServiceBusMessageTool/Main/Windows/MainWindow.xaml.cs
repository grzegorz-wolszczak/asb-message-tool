using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Main.Application;
using Main.ViewModels;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;

namespace Main.Windows
{
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


         leftPanelControl.leftPanelServiceBusConfigTabItem.DataContext = leftPanelControlViewModel;
         leftPanelControl.leftPanelSendersConfigTabItem.DataContext = leftPanelControlViewModel;
         leftPanelControl.leftPanelReceiversConfigTabItem.DataContext = leftPanelControlViewModel;
         leftPanelControl.leftPanelServiceBusConfigUserControl.DataContext = serviceBusConfigsViewModel;

         //rightPanelControl.rightPanelServiceBusConfigTab.DataContext = serviceBusConfigsViewModel;
         leftPanelControl.leftPanelSendersConfigUserControl.DataContext = sendersViewModel;
         rightPanelControl.rightPanelSenderConfigTab.DataContext = sendersViewModel;

         leftPanelControl.leftPanelReceiversConfigUserControl.DataContext = receiversViewModel;
         rightPanelControl.rightPanelReceiverConfigTab.DataContext = receiversViewModel;

         Title = $"{AppConstants.ApplicationName} ({AppConstants.Version})";
      }

      private void LogContentTextBoxChanged(object sender, TextChangedEventArgs e)
      {
         if (((ViewModels.MainViewModel) this.DataContext).ShouldScrollToEndOnLogContentChange)
         {
            this.logContentTextBox.ScrollToEnd();
         }
      }

      private void LogContentTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
      {
         if (Keyboard.Modifiers != ModifierKeys.Control)
         {
            return;
         }

         var viewModel = (MainViewModel) DataContext;

         e.Handled = true;
         if (e.Delta > 0)
         {
            ++viewModel.LogTextBoxFontSize;
         }
         else
         {
            --viewModel.LogTextBoxFontSize;
         }
      }
   }
}
