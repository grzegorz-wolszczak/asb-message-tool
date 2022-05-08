using System.Windows.Controls;
using System.Windows.Input;
using ASBMessageTool.Application.Config;

namespace ASBMessageTool.ReceivingMessages.Gui;

public partial class ReceiverConfigUserControl : UserControl
{
    public ReceiverConfigUserControl()
    {
        InitializeComponent();
    }

    private void ReceivedMessagesTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        e.Handled = true;

        ReceiverConfigViewModel item = (ReceiverConfigViewModel)DataContext;
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

    private void ReceivedMessageTextBoxBoxChanged(object sender, TextChangedEventArgs e)
    {
        ReceivedMessagesTextBox.ScrollToEnd();
    }
}
