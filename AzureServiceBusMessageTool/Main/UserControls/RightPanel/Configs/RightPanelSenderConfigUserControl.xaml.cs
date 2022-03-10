using System;
using System.Windows.Controls;
using System.Windows.Input;
using Main.Application;
using Main.ViewModels.Configs.Senders;

namespace Main.UserControls.RightPanel.Configs;

public partial class DetailedSenderConfigUserControl : UserControl
{
    public DetailedSenderConfigUserControl()
    {
        InitializeComponent();
    }

    // todo: THIS IS DIRTY CODE - USING REFLECTION ONLY BECAUSE THIS USER CONTROL HAVE DIFFERENT DATACONTEXT SET WITH TWO
    // DIFFERENT CASE, ONE WHEN IT IS EMBEDDED, AND ONE WHEN IT IS DISPLAYED IN A WINDOW
    // TODO: FIX THIS GLOBALLY AND MAKE WINDOW AND EMBEDDED CONTROL HAVING THE SAME DATA CONTEXT SET

    private void SenderMsgBodyTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        e.Handled = true;
        var propertyName = $"{nameof(SendersSelectedConfigViewModel.CurrentSelectedConfigModelItem)}";
        var propertyInfo = DataContext.GetType().GetProperty(propertyName);
        SenderConfigViewModel item = (SenderConfigViewModel) propertyInfo.GetValue(DataContext, null);
        var targetItem = item.Item;
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

    // avalonEdit does not have its property 'Text' as dependency property so we cannot bind viewmodel for it
    // must set MsgBody in code behind;

    private void SenderMsgBodyTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (DataContext == null)
        {
            return;
        }
        var propertyName = $"{nameof(SendersSelectedConfigViewModel.CurrentSelectedConfigModelItem)}";
        var propertyInfo = DataContext.GetType().GetProperty(propertyName);

        SenderConfigViewModel item = (SenderConfigViewModel) propertyInfo.GetValue(DataContext, null);

        if (item != null)
        {
            item.Item.MsgBody = SenderMsgBodyTextBox.Text;
        }
    }


}