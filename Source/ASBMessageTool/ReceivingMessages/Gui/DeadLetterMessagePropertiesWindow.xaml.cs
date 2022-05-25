using System;
using System.ComponentModel;
using System.Windows;
using ASBMessageTool.ReceivingMessages.Code;

namespace ASBMessageTool.ReceivingMessages.Gui;

public partial class DeadLetterMessagePropertiesWindow : Window
{
    private DeadLetterMessagePropertiesViewModel _viewModel;

    public DeadLetterMessagePropertiesWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        MessageApplicationPropertiesVerifications.WarnUserOnDuplicatesExistence(_viewModel, Hide);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }


    public void ShowDialogForDataContext(DeadLetterMessagePropertiesViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        ShowDialog();
    }
}
