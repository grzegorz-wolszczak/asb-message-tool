using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Main.Application;
using Main.ViewModels;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;

namespace Main.Windows.MainWindow;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double lastExpandedExpanderHeight;

    public MainWindow(
        MainViewModel mainViewModel,
        ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel,
        SendersSelectedConfigViewModel sendersViewModel,
        ReceiversSelectedConfigViewModel receiversViewModel,
        LeftPanelControlViewModel leftPanelControlViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;

        leftPanelControl.DataContext = leftPanelControlViewModel;


        leftPanelControl.leftPanelServiceBusConfigTabItem.DataContext = leftPanelControlViewModel;
        leftPanelControl.leftPanelSendersConfigTabItem.DataContext = leftPanelControlViewModel;
        leftPanelControl.leftPanelReceiversConfigTabItem.DataContext = leftPanelControlViewModel;
        leftPanelControl.leftPanelServiceBusConfigUserControl.DataContext = serviceBusConfigsViewModel;

        leftPanelControl.leftPanelSendersConfigUserControl.DataContext = sendersViewModel;
        rightPanelControl.rightPanelSenderConfigTab.DataContext = sendersViewModel;

        leftPanelControl.leftPanelReceiversConfigUserControl.DataContext = receiversViewModel;
        rightPanelControl.rightPanelReceiverConfigTab.DataContext = receiversViewModel;

        Title = $"{StaticConfig.ApplicationName} ({VersionConfig.Version})";
    }

    private void LogContentTextBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (((MainViewModel) DataContext).ShouldScrollToEndOnLogContentChange)
        {
            LogContentTextBox.ScrollToEnd();
        }
    }

    private void LogContentTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        var viewModel = (MainViewModel) DataContext;

        e.Handled = true;
        var value = viewModel.LogTextBoxFontSize;

        if (e.Delta > 0)
        {
            ++value;
        }
        else
        {
            --value;
        }

        if (value < AppDefaults.MinimumTextBoxFontSize)
        {
            value = AppDefaults.MinimumTextBoxFontSize;
        }

        viewModel.LogTextBoxFontSize = value;
    }

    private void BottomExpander_OnCollapsed(object sender, RoutedEventArgs e)
    {
        MainContentRow.Height = new GridLength(1, GridUnitType.Star);
        ExpanderRow.Height = new GridLength(1, GridUnitType.Auto);
    }

    private void BottomExpander_OnExpanded(object sender, RoutedEventArgs e)
    {
        // if it was never assigned , it will be zero
        // in that case calculate ExpanderRow differently
        if (lastExpandedExpanderHeight <= 0)
        {
            ExpanderRow.Height = new GridLength(0.3, GridUnitType.Star);
        }
        else
        {
            ExpanderRow.Height = new GridLength(lastExpandedExpanderHeight);

        }
    }

    private void BottomExpander_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (!LogTextBoxExpander.IsExpanded)
        {
            return;
        }

        if (!e.HeightChanged)
        {
            return;
        }

        lastExpandedExpanderHeight = e.NewSize.Height;
    }
}
