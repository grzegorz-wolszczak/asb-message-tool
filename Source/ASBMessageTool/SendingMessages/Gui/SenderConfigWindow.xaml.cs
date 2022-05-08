using System;
using System.ComponentModel;
using System.Windows;

namespace ASBMessageTool.SendingMessages.Gui;

public partial class SenderConfigWindow : Window
{
    private readonly Action _onCloseAction;


    public SenderConfigWindow(Action onCloseAction)
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

