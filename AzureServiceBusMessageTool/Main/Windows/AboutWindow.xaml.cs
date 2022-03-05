using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Main.ViewModels;

namespace Main.Windows
{
   public partial class AboutWindow : Window
   {
      public AboutWindow(AboutWindowViewModel aboutWindowViewModel)
      {
         InitializeComponent();

         DataContext = aboutWindowViewModel;

      }

      // code from here : https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf
      private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
      {
         // for .NET Core you need to add UseShellExecute = true
         // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
         Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
         {
            UseShellExecute = true
         });
         e.Handled = true;
      }

      protected override void OnClosing(CancelEventArgs e)
      {
         e.Cancel = true;
         Hide();
      }
   }
}

