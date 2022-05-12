using System;
using System.Windows.Input;
using ASBMessageTool.Application.Config;

namespace ASBMessageTool.Gui;

public static class GuiElementsHelperRoutines
{
    public static void ChangeValueOnMouseWheelEventWithCtrlKeyPressed(
        Func<int> getValueFunc, 
        Action<int> setValueFunc, object sender, MouseWheelEventArgs e )
    {
        
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        e.Handled = true;
        var value = getValueFunc();

        if (e.Delta > 0)
        {
            ++value;
        }
        else
        {
            --value;
        }

        if (value < AppDefaults.MinimumTextBoxFontSize)
        {
            value = AppDefaults.MinimumTextBoxFontSize;
        }

        setValueFunc(value);
    }
}
