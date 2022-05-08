using System;
using System.Windows;

namespace ASBMessageTool.Application;
using static ASBMessageTool.Application.NativeMethods;
public static class UserInteractions
{
    private static readonly IntPtr TaskDialogHandle = new System.Windows.Interop.WindowInteropHelper(new Window()).Handle;
    
    public static void ShowErrorDialog(string reason, string messageBoxTest)
    {
        TaskDialog(TaskDialogHandle,
            TaskDialogHandle,
            "Error", // title
            reason, // main instruction
            messageBoxTest, // content
            TaskDialogButtons.Ok,
            TaskDialogIcon.Error);
    }

    public static void ShowExceptionDialog(Exception exception)
    {
        TaskDialog(TaskDialogHandle,
            TaskDialogHandle,
            "Error",
            $"Unhandled exception\n\nApplication will close",
            exception.ToString(),
            TaskDialogButtons.Ok,
            TaskDialogIcon.Error);
    }
}
