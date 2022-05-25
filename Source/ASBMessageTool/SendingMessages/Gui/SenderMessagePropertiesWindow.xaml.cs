using System;
using System.ComponentModel;
using System.Windows;
using ASBMessageTool.Model;
using ASBMessageTool.ReceivingMessages.Code;

namespace ASBMessageTool.SendingMessages.Gui;

public partial class SenderMessagePropertiesWindow : Window
{
    private SbMessageFieldsViewModel _viewModel;

    public SenderMessagePropertiesWindow()
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


    public void ShowDialogForDataContext(SbMessageFieldsViewModel dataContext)
    {
        _viewModel = dataContext;
        DataContext = dataContext;
        ShowDialog();
    }
}
