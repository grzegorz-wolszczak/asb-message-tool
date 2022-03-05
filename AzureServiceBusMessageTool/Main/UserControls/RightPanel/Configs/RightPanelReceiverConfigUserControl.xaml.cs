using System.Windows;
using System.Windows.Controls;
using Microsoft.Azure.Amqp.Framing;

namespace Main.UserControls.RightPanel.Configs;

public partial class RightPanelReceiverConfigUserControl : UserControl
{
   public RightPanelReceiverConfigUserControl()
   {
      InitializeComponent();
   }

   private void ReceivedMessageTextBoxBoxChanged(object sender, TextChangedEventArgs e)
   {

      if (DataContext is Main.ViewModels.Configs.Receivers.ReceiversSelectedConfigViewModel viewModel1)
      {
         if (viewModel1.CurrentSelectedConfigModelItem.Item.ShouldScrollTextBoxToEndOnNewMessageReceive)
         {
            receivedMessagesTextBox.ScrollToEnd();
         }
      }
      else if (DataContext is Main.ViewModels.Configs.Receivers.ReceiverConfigViewModelWrapper viewModel2)
      {
         if (viewModel2.CurrentSelectedConfigModelItem.Item.ShouldScrollTextBoxToEndOnNewMessageReceive)
         {
            receivedMessagesTextBox.ScrollToEnd();
         }
      }
   }
}

