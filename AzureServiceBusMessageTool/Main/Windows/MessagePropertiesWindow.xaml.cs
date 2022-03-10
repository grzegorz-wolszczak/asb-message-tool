using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Main.ViewModels.Configs.Senders;

namespace Main.Windows;

public partial class MessagePropertiesWindow : Window
{
    private SbMessageFieldsViewModel _viewModel;

    public MessagePropertiesWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;

        if (_viewModel == null)
        {
            return;
        }

        _viewModel.RemoveEmptyProperties();
        var duplicatedPropertyNames = _viewModel.GetDuplicatedApplicationProperties();
        if (duplicatedPropertyNames.Count > 0)
        {
            var duplicatedNames = string.Join(", ", duplicatedPropertyNames.Select(e=>$"\"{e}\""));
            Xceed.Wpf.Toolkit.MessageBox.Show(this,
                $"Property names must be unique.\n" +
                $"Found following duplicate names:\n\n{duplicatedNames}" +
                $"\n\nRemove duplicates or rename them.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        else
        {
            Hide();
        }
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
