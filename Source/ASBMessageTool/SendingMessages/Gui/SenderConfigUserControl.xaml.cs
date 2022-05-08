using System;
using System.Windows.Controls;
using System.Windows.Input;
using ASBMessageTool.Application.Config;

namespace ASBMessageTool.SendingMessages.Gui;

public partial class SenderConfigUserControl : UserControl
{
    public SenderConfigUserControl()
    {
        InitializeComponent();
    }

    // avalonEdit does not have its property 'Text' as dependency property so we cannot bind viewmodel for it
    // must set MsgBody in code behind;

    private void SenderMsgBodyTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (DataContext is null)
        {
            return;
        }

        if (DataContext is SenderConfigViewModel model)
        {
            if (model.ModelItem is not null)
            {
                model.ModelItem.MsgBody = SenderMsgBodyTextBox.Text;
            }
        }
    }

    private void SenderMsgBodyTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        e.Handled = true;

        if (DataContext is null)
        {
            return;
        }

        if (DataContext is SenderConfigViewModel item)
        {
            var targetItem = item.ModelItem;
            var value = targetItem.MsgBodyTextBoxFontSize;
            if (e.Delta > 0)
            {
                value++;
            }
            else
            {
                value--;
            }

            if (value < AppDefaults.MinimumTextBoxFontSize)
            {
                value = AppDefaults.MinimumTextBoxFontSize;
            }

            targetItem.MsgBodyTextBoxFontSize = value;
        }
    }
}
