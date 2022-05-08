using System;
using System.ComponentModel;
using System.Windows;

namespace ASBMessageTool.ReceivingMessages.Gui;

public partial class ReceiverConfigWindow : Window
{
    private readonly Action _onCloseAction;


    public ReceiverConfigWindow(Action onCloseAction)
    {
        _onCloseAction = onCloseAction;
        InitializeComponent();
    }

    public bool ShouldHideOnClose { get; set; } = true;

    protected override void OnClosing(CancelEventArgs e)
    {

        if (ShouldHideOnClose)
        {
            e.Cancel = true;
            Hide();
            _onCloseAction.Invoke();
        }
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }
}

