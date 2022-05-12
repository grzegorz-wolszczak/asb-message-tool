using System;
using System.Windows;
using System.Windows.Threading;
using ASBMessageTool.Application;

namespace ASBMessageTool;

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
        var shutdowner = new ApplicationShutdowner(Current);
        _applicationLogicRoot = new ApplicationLogicRoot(shutdowner);
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
        UserInteractions.ShowExceptionDialog($"Unhandled exception\n\nApplication will close", e.Exception);
        Current?.Shutdown();
    }

}

public interface IApplicationShutdowner
{
    void Shutdown();
}

public class ApplicationShutdowner : IApplicationShutdowner
{
    private readonly System.Windows.Application _application;

    public ApplicationShutdowner(System.Windows.Application application)
    {
        _application = application;
    }

    public void Shutdown()
    {
        _application?.Shutdown();
        Environment.Exit(1);
    }
}
