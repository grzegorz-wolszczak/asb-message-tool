using System;
using System.ComponentModel;
using System.Windows;
using Main.ViewModels.Configs.Senders;

namespace Main.Windows.Configs;

public partial class SenderConfigWindow : Window
{
    private SenderConfigViewModelWrapper _currentSelectedItem;

    public SenderConfigWindow(SenderConfigViewModelWrapper currentSelectedItem)
    {
        _currentSelectedItem = currentSelectedItem;

        InitializeComponent();
        DataContext = currentSelectedItem;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
        _currentSelectedItem.CurrentSelectedConfigModelItem.IsEmbeddedInsideRightPanel = true;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }
}