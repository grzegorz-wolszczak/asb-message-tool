using System.Windows;
using System.Windows.Threading;
using Main.Application;
using Main.ExceptionHandling;

namespace Main;

public partial class App : System.Windows.Application
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


    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;

        MessageBox.Show(
            $"Unhandled exception happened {e.Exception}\n\nApplication will close",
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        Current?.Shutdown();
    }
}
