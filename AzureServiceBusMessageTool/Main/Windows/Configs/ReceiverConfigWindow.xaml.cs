using System;
using System.ComponentModel;
using System.Windows;
using Main.ViewModels.Configs.Receivers;

namespace Main.Windows.Configs
{
   public partial class ReceiverConfigWindow : Window
   {

      private ReceiverConfigViewModelWrapper _currentSelectedItem;

      public ReceiverConfigWindow(ReceiverConfigViewModelWrapper currentSelectedItem)
      {
         _currentSelectedItem = currentSelectedItem;

         InitializeComponent();
         DataContext = currentSelectedItem;
      }

      protected override void OnClosing(CancelEventArgs e)
      {
         e.Cancel = true;
         Hide();
         _currentSelectedItem.CurrentSelectedConfigModelItem.IsEmbeddedInsideRightPanel = true;
      }

      protected override void OnInitialized(EventArgs e)
      {
         base.OnInitialized(e);

         System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
      }
   }
}

