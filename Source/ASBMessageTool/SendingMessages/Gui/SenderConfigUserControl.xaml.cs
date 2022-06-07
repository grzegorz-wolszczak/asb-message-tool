using System;
using System.Windows.Controls;
using System.Windows.Input;
using ASBMessageTool.Gui;
using ASBMessageTool.SendingMessages.Code;

namespace ASBMessageTool.SendingMessages.Gui;

public partial class SenderConfigUserControl : UserControl
{
    public SenderConfigUserControl()
    {
        InitializeComponent();
    }

    // avalonEdit does not have its property 'Text' as dependency property so we cannot bind viewmodel for it,
    // we must set MsgBody in code behind;

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
        SenderConfigViewModel item = (SenderConfigViewModel)DataContext;
        var targetItem = item.ModelItem;

        GuiElementsHelperRoutines.ChangeValueOnMouseWheelEventWithCtrlKeyPressed(
            () => targetItem.MsgBodyTextBoxFontSize,
            value => { targetItem.MsgBodyTextBoxFontSize = value; },
            e);
    }
}
