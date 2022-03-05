using Main.ViewModels;
using Main.Windows;

namespace Main.Application
{
   public class AboutWindowProxy: IAboutWindowProxy
   {
      private readonly AboutWindow _aboutWindow;

      public AboutWindowProxy(AboutWindow aboutWindow)
      {
         _aboutWindow = aboutWindow;
      }

      public void Show()
      {
         _aboutWindow.ShowDialog();
      }
   }
}
