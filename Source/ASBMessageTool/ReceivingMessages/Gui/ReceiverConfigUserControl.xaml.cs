using System.Windows.Controls;
using System.Windows.Input;
using ASBMessageTool.Gui;
using ASBMessageTool.ReceivingMessages.Code;
using Xceed.Wpf.Toolkit.Core.Input;

namespace ASBMessageTool.ReceivingMessages.Gui;

public partial class ReceiverConfigUserControl : UserControl
{
    public ReceiverConfigUserControl()
    {
        InitializeComponent();
    }

    private void ReceivedMessagesTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        
        ReceiverConfigViewModel item = (ReceiverConfigViewModel)DataContext;
        var targetItem = item.ModelItem;
        
        GuiElementsHelperRoutines.ChangeValueOnMouseWheelEventWithCtrlKeyPressed(
            () => targetItem.MsgBodyTextBoxFontSize,
            value => { targetItem.MsgBodyTextBoxFontSize = value; },
            sender, 
            e);
    }

    private void ReceivedMessageTextBoxBoxChanged(object sender, TextChangedEventArgs e)
    {
        ReceivedMessagesTextBox.ScrollToEnd();
    }

    private void UpDownBase_OnInputValidationError(object sender, InputValidationErrorEventArgs e)
    {
        e.ThrowException = false;
        //MessageBox.Show($"Validation error, {e.Exception}");
    }
}
