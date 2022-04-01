using System;
using System.Windows;

namespace ASBMessageTool.Application;
using static NativeMethods;
public static class UserInteractions
{
    private static readonly IntPtr TaskDialogHandle = new System.Windows.Interop.WindowInteropHelper(new Window()).Handle;
    
    public static void ShowErrorDialog(string reason, string messageBoxTest)
    {
        var taskHandle = GetTaskHandle();
        TaskDialog(taskHandle,
            taskHandle,
            "Error", // title
            reason, // main instruction
            messageBoxTest, // content
            TaskDialogButtons.Ok,
            TaskDialogIcon.Error);
    }

    private static IntPtr GetTaskHandle()
    {
        if (System.Windows.Application.Current?.MainWindow is not null)
        {
            var window = System.Windows.Application.Current.MainWindow;
            return new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }
        else
        {
            return TaskDialogHandle;
        }
    }

    public static void ShowExceptionDialog(string reason, Exception exception)
    {
        var taskHandle = GetTaskHandle();
        TaskDialog(taskHandle,
            taskHandle,
            "Error",
            reason,
            exception.ToString(),
            TaskDialogButtons.Ok,
            TaskDialogIcon.Error);
    }

    public static TaskDialogResult ShowYesNoQueryDialog(string reason, string message)
    {
        return ShowYesNoQueryDialog(reason, message, "Error");
    }
    
    public static TaskDialogResult ShowYesNoQueryDialog(string reason, string message, string windowTitle)
    {
        var taskHandle = GetTaskHandle();
        return TaskDialog(taskHandle,
            taskHandle,
            windowTitle,
            reason,
            message,
            TaskDialogButtons.Yes | TaskDialogButtons.No,
            TaskDialogIcon.Error);
    }
}
