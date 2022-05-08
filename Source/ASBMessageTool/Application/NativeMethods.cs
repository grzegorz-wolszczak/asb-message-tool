using System;
using System.Runtime.InteropServices;

namespace ASBMessageTool.Application;

public static class NativeMethods
{
    [DllImport("comctl32.dll", PreserveSig = false, CharSet = CharSet.Unicode)]
    // Note: setting [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)] breaks application and does not show dialogs
    // disabling with pragma
#pragma warning disable CA5392
    public static extern TaskDialogResult TaskDialog(IntPtr hwndParent, IntPtr hInstance,
#pragma warning restore CA5392
        string title, string mainInstruction, string content,
        TaskDialogButtons buttons, TaskDialogIcon icon);

    public enum TaskDialogResult
    {
        None = 0,
        Ok = 1,
        Cancel = 2,
        Retry = 4,
        Yes = 6,
        No = 7,
        Close = 8
    }

    [Flags]
    public enum TaskDialogButtons
    {
        None = 0,
        Ok = 0x0001,
        Yes = 0x0002,
        No = 0x0004,
        Cancel = 0x0008,
        Retry = 0x0010,
        Close = 0x0020
    }

    public enum TaskDialogIcon
    {
        None = 0,
        Warning = 65535,
        Error = 65534,
        Information = 65533,
        Shield = 65532
    }
}
