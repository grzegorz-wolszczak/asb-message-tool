using System.Windows;
using Main.Application;
using Main.ExceptionHandling;


namespace Main
{
   public partial class App
   {
      private ApplicationLogicRoot _applicationLogicRoot;
      private readonly WindowExceptionHandler _exceptionHandler;

      public App()
      {
         _exceptionHandler = new WindowExceptionHandler();
      }

      private void App_OnStartup(object sender, StartupEventArgs e)
      {
         _applicationLogicRoot = new ApplicationLogicRoot(new ApplicationProxy(this));
         _applicationLogicRoot.Start();
      }

      private void App_OnExit(object sender, ExitEventArgs e)
      {
         _applicationLogicRoot.Stop();

      }

   }
}