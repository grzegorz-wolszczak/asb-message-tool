using System;
using System.ComponentModel;
using System.Windows;

namespace Main.Windows;

public partial class MessagePropertiesWindow : Window
{
   public MessagePropertiesWindow()
   {
      InitializeComponent();
   }

   protected override void OnClosing(CancelEventArgs e)
   {
      e.Cancel = true;
      Hide();
   }

   protected override void OnInitialized(EventArgs e)
   {
      base.OnInitialized(e);

      System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
   }
}

