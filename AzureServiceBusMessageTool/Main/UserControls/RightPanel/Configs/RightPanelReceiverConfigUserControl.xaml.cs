using System.Windows.Controls;
using System.Windows.Input;
using Main.Application;
using Main.ViewModels.Configs.Receivers;

namespace Main.UserControls.RightPanel.Configs;

public partial class RightPanelReceiverConfigUserControl : UserControl
{
    public RightPanelReceiverConfigUserControl()
    {
        InitializeComponent();
    }

    private void ReceivedMessageTextBoxBoxChanged(object sender, TextChangedEventArgs e)
    {

        if (DataContext is Main.ViewModels.Configs.Receivers.ReceiversSelectedConfigViewModel viewModel1)
        {
            if (viewModel1.CurrentSelectedConfigModelItem.Item.ShouldScrollTextBoxToEndOnNewMessageReceive)
            {
                ReceivedMessagesTextBox.ScrollToEnd();
            }
        }
        else if (DataContext is Main.ViewModels.Configs.Receivers.ReceiverConfigViewModelWrapper viewModel2)
        {
            if (viewModel2.CurrentSelectedConfigModelItem.Item.ShouldScrollTextBoxToEndOnNewMessageReceive)
            {
                ReceivedMessagesTextBox.ScrollToEnd();
            }
        }
    }

    private void ReceivedMessagesTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        e.Handled = true;
        var propertyName = $"{nameof(ReceiversSelectedConfigViewModel.CurrentSelectedConfigModelItem)}";
        var propertyInfo = DataContext.GetType().GetProperty(propertyName);
        ReceiverConfigViewModel item = (ReceiverConfigViewModel)propertyInfo.GetValue(DataContext, null);
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
}