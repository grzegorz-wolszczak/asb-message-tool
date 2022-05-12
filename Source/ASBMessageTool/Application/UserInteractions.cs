using System;
using System.Windows;
using TaskDialogInterop;

namespace ASBMessageTool.Application;

public static class UserInteractions
{
    public enum YesNoDialogResult
    {
        Yes =0,
        No =2,
    }
    
    private static Window _windowNotOwnedByApplication;
    private static Window _mainWindow;
    private static IntPtr _mainWindowTaskDialogHandle = IntPtr.Zero;

    private static IntPtr _taskDialogHandle = IntPtr.Zero;

    public static void ShowErrorDialog(string reason, string messageBoxTest)
    {
        TaskDialogOptions options = new TaskDialogOptions();
        options.Content = messageBoxTest;
        options.Title = reason;
        options.MainIcon = VistaTaskDialogIcon.Error;
        options.Owner = GetWindow();

        TaskDialogInterop.TaskDialog.Show(options);
    }

    private static Window GetWindow()
    {
        if (_mainWindow is not null)
        {
            return _mainWindow;
        }

        if (System.Windows.Application.Current?.MainWindow is not null)
        {
            _mainWindow = System.Windows.Application.Current.MainWindow;
            return _mainWindow;
        }

        if (_windowNotOwnedByApplication is null)
        {
            _windowNotOwnedByApplication = new Window();
        }

        return _windowNotOwnedByApplication;
    }

    private static IntPtr GetTaskHandle()
    {
        if (_mainWindowTaskDialogHandle != IntPtr.Zero)
        {
            return _mainWindowTaskDialogHandle;
        }

        if (System.Windows.Application.Current?.MainWindow is not null)
        {
            if (_mainWindow is null)
            {
                _mainWindow = System.Windows.Application.Current.MainWindow;
            }

            _mainWindowTaskDialogHandle = new System.Windows.Interop.WindowInteropHelper(_mainWindow).Handle;

            return _mainWindowTaskDialogHandle;
        }


        if (_taskDialogHandle == IntPtr.Zero)
        {
            _taskDialogHandle = new System.Windows.Interop.WindowInteropHelper(GetWindow()).Handle;
        }


        return _taskDialogHandle;
    }

    public static void ShowExceptionDialog(string reason, Exception exception)
    {
        // var taskHandle = GetTaskHandle();
        // TaskDialog(taskHandle,
        //     taskHandle,
        //     "Error",
        //     reason,
        //     exception.ToString(),
        //     TaskDialogButtons.Ok,
        //     TaskDialogIcon.Error);
        //
        TaskDialogOptions options = new TaskDialogOptions();
        options.Content = exception.Message;
        options.ExpandedInfo = exception.ToString();
        options.Title = reason;
        options.MainIcon = VistaTaskDialogIcon.Error;
        options.Owner = GetWindow();

        TaskDialogInterop.TaskDialog.Show(options);
    }

    // public static YesNoDialogResult ShowYesNoQueryDialog(string reason, string message)
    // {
    //     return ShowYesNoQueryDialog(reason, message, "Error");
    // }

    public static YesNoDialogResult ShowYesNoQueryDialog(string reason, string message, string windowTitle, Exception exception)
    {
        TaskDialogOptions options = TaskDialogOptions.Default;

        options.Owner = GetWindow();
        options.Title = windowTitle;
        options.Content = message;
        options.ExpandedInfo = exception.ToString();
        options.CommonButtons = TaskDialogCommonButtons.YesNo;
        options.MainIcon = VistaTaskDialogIcon.Error;

        var result2 =  TaskDialogInterop.TaskDialog.Show(options).Result;
        
        //
        // var result2 = TaskDialogInterop.TaskDialog.ShowMessage(
        //     GetWindow(),
        //     message,
        //     reason,
        //     TaskDialogCommonButtons.YesNo,
        //     VistaTaskDialogIcon.Error);
        
        
        return result2 ==  TaskDialogSimpleResult.Yes ? YesNoDialogResult.Yes : YesNoDialogResult.No;
        
        
        /*
         *     public static TaskDialogSimpleResult ShowMessage(Window owner, string messageText, string caption, TaskDialogCommonButtons buttons, VistaTaskDialogIcon icon)
    {
        TaskDialogOptions options = TaskDialogOptions.Default;

        options.Owner = owner;
        options.Title = caption;
        options.Content = messageText;
        options.CommonButtons = buttons;
        options.MainIcon = icon;

        return Show(options).Result;
    }
         */
    }

    public static void ShowInformationDialog(string reason, string message)
    {
        TaskDialogOptions options = new TaskDialogOptions();
        options.Content = message;
        options.Title = reason;
        options.MainIcon = VistaTaskDialogIcon.Information;
        options.Owner = GetWindow();

        TaskDialogInterop.TaskDialog.Show(options);
    }
}
