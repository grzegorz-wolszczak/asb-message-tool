using System.Windows.Controls;
using System.Windows.Input;
using ASBMessageTool.Gui;
using ASBMessageTool.PeekingMessages.Code;

namespace ASBMessageTool.PeekingMessages.Gui;

public partial class PeekerConfigUserControl : UserControl
{
    public PeekerConfigUserControl()
    {
        InitializeComponent();
    }

    private void ReceivedMessagesTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        PeekerConfigViewModel item = (PeekerConfigViewModel)DataContext;
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
}

