using System;
using System.Windows;
using System.Windows.Threading;
using ASBMessageTool.Application;

namespace ASBMessageTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            _applicationLogicRoot = new ApplicationLogicRoot();
            _applicationLogicRoot.Start();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            // this can be null if we exception occured in OnStartup and applicationLogicRootObject was never created
            _applicationLogicRoot?.Stop();
        }


        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            UserInteractions.ShowExceptionDialog(e.Exception);
            Current?.Shutdown();
        }

    }
}
