using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Model;

namespace ASBMessageTool.Gui.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private double _lastExpandedExpanderHeight;
    private readonly MainViewModel _mainViewModel;

    public MainWindow(MainViewModel mainWindowViewModel,
        LeftRightTabsSyncViewModel leftRightTabsSyncViewModel,
        LeftPanelSendersConfigControlViewModel leftPanelSendersConfigControlViewModel,
        RightPanelSendersConfigControlViewModel rightPanelSendersConfigControlViewModel,
        LeftPanelReceiversConfigControlViewModel leftPanelReceiversConfigControlViewModel,
        RightPanelReceiversConfigControlViewModel rightPanelReceiversConfigControlViewModel)
    {
        InitializeComponent();
        _mainViewModel = mainWindowViewModel;
        DataContext = mainWindowViewModel;
        LeftPanelControl.DataContext = leftRightTabsSyncViewModel;
        LeftPanelControl.LeftPanelSendersConfigTab.DataContext = leftPanelSendersConfigControlViewModel;
        LeftPanelControl.LeftPanelReceiversConfigTab.DataContext = leftPanelReceiversConfigControlViewModel;

        RightPanelControl.DataContext = leftRightTabsSyncViewModel;
        RightPanelControl.RightPanelSendersTabItem.DataContext = rightPanelSendersConfigControlViewModel;
        RightPanelControl.RightPanelReceiversTabItem.DataContext = rightPanelReceiversConfigControlViewModel;
        
        Title = $"{StaticConfig.ApplicationName} ({VersionConfig.Version})";
    }

    private void LogContentTextBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (_mainViewModel.ShouldScrollToEndOnLogContentChange)
        {
            LogContentTextBox.ScrollToEnd();
        }
    }

    // increase/decrease log textbox font size when on CTRL+<MouseWheel>
    private void LogContentTextBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        GuiElementsHelperRoutines.ChangeValueOnMouseWheelEventWithCtrlKeyPressed(
            () => (int)LogContentTextBox.FontSize,
            value => { LogContentTextBox.FontSize = value; },
            sender, 
            e);
        
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
        if (_lastExpandedExpanderHeight <= 0)
        {
            ExpanderRow.Height = new GridLength(0.3, GridUnitType.Star);
        }
        else
        {
            ExpanderRow.Height = new GridLength(_lastExpandedExpanderHeight);
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

        _lastExpandedExpanderHeight = e.NewSize.Height;
    }
}
