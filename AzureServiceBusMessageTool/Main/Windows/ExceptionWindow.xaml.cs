using System;
using System.Windows;

namespace Main.Windows
{
   public partial class ExceptionWindow : Window
   {
      public ExceptionWindow()
      {
         InitializeComponent();
      }

      private void OnExceptionWindowClosed(object? sender, EventArgs e)
      {
         System.Windows.Application.Current.Shutdown();
      }

      private void OnExitAppClick(object sender, RoutedEventArgs e)
      {
         System.Windows.Application.Current.Shutdown();
      }
   }
}

