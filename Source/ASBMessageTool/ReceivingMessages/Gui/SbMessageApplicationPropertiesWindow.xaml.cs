using System;
using System.ComponentModel;
using System.Windows;

namespace ASBMessageTool.ReceivingMessages.Gui;

public partial class SbMessageApplicationPropertiesWindow : Window
{
    private SbMessageApplicationPropertiesViewModel _viewModel;
    public SbMessageApplicationPropertiesWindow()
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

    public void ShowDialogForDataContext(SbMessageApplicationPropertiesViewModel dataContext)
    {
        _viewModel = dataContext;
        DataContext = dataContext;
        ShowDialog();
    }
}
