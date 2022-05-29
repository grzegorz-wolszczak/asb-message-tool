﻿using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace TaskDialogInterop;

/// <summary>
/// Displays a task dialog.
/// </summary>
public partial class TaskDialog : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskDialog"/> class.
    /// </summary>
    public TaskDialog()
    {
        InitializeComponent();

        Loaded += new RoutedEventHandler(TaskDialog_Loaded);
        SourceInitialized += new EventHandler(TaskDialog_SourceInitialized);
        KeyDown += new KeyEventHandler(TaskDialog_KeyDown);
        ContentRendered += new EventHandler(TaskDialog_ContentRendered);
        Closing += new System.ComponentModel.CancelEventHandler(TaskDialog_Closing);
        base.Closed += new EventHandler(TaskDialog_Closed);
    }

    private TaskDialogViewModel ViewModel
    {
        get { return DataContext as TaskDialogViewModel; }
    }

    private void TaskDialog_Loaded(object sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);
            ViewModel.RequestClose +=new EventHandler(ViewModel_RequestClose);

            ConvertToHyperlinkedText(ContentText, ViewModel.Content);
            ConvertToHyperlinkedText(ContentExpandedInfo, ViewModel.ContentExpandedInfo);
            ConvertToHyperlinkedText(FooterExpandedInfo, ViewModel.FooterExpandedInfo);
            ConvertToHyperlinkedText(FooterText, ViewModel.FooterText);

            WindowStartupLocation = ViewModel.StartPosition;

            if (ViewModel.NormalButtons.Count == 0)
            {
                MaxWidth = 462;
            }
				
            // Footer only shows the secondary white top border when the buttons section is visible
            FooterInner.BorderThickness = new Thickness(
                FooterInner.BorderThickness.Left,
                ((ButtonsArea.Visibility == Visibility.Visible) ? 1 : 0),
                FooterInner.BorderThickness.Right,
                FooterInner.BorderThickness.Bottom);

            // Hide the special button areas if they are empty
            if (ViewModel.CommandLinks.Count == 0)
                CommandLinks.Visibility = Visibility.Collapsed;
            if (ViewModel.RadioButtons.Count == 0)
                RadioButtons.Visibility = Visibility.Collapsed;

            // Play the appropriate sound
            switch (ViewModel.MainIconType)
            {
                default:
                case VistaTaskDialogIcon.None:
                case VistaTaskDialogIcon.Shield:
                    // No sound
                    break;
                case VistaTaskDialogIcon.Warning:
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
                case VistaTaskDialogIcon.Error:
                    System.Media.SystemSounds.Hand.Play();
                    break;
                case VistaTaskDialogIcon.Information:
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
            }
        }
    }
    private void TaskDialog_SourceInitialized(object sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.NotifyConstructed();
            ViewModel.NotifyCreated();

            if (ViewModel.AllowDialogCancellation)
            {
                SafeNativeMethods.SetWindowIconVisibility(this, false);
            }
            else
            {
                // This also hides the icon, too
                SafeNativeMethods.SetWindowCloseButtonVisibility(this, false);
            }
        }
    }
    private void TaskDialog_KeyDown(object sender, KeyEventArgs e)
    {
        if (ViewModel != null)
        {
            // Block Alt-F4 if it isn't allowed
            if (!ViewModel.AllowDialogCancellation
                && e.Key == Key.System && e.SystemKey == Key.F4)
                e.Handled = true;

            // Handel Esc manually if the override has been set
            if (ViewModel.AllowDialogCancellation
                && e.Key == Key.Escape)
            {
                e.Handled = true;
                DialogResult = false;
                Close();
            }
        }
    }
    private void TaskDialog_ContentRendered(object sender, EventArgs e)
    {
        ViewModel.NotifyShown();
    }
    private void TaskDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = ViewModel.ShouldCancelClosing();
    }
    private void TaskDialog_Closed(object sender, EventArgs e)
    {
        ViewModel.NotifyClosed();
    }
    private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Content")
            ConvertToHyperlinkedText(ContentText, ViewModel.Content);
        if (e.PropertyName == "ContentExpandedInfo")
            ConvertToHyperlinkedText(ContentExpandedInfo, ViewModel.ContentExpandedInfo);
        if (e.PropertyName == "FooterExpandedInfo")
            ConvertToHyperlinkedText(FooterExpandedInfo, ViewModel.FooterExpandedInfo);
        if (e.PropertyName == "FooterText")
            ConvertToHyperlinkedText(FooterText, ViewModel.FooterText);
    }
    private void ViewModel_RequestClose(object sender, EventArgs e)
    {
        Close();
    }
    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
    }
    private void CommandLink_Click(object sender, RoutedEventArgs e)
    {
    }
    private void Hyperlink_Click(object sender, EventArgs e)
    {
        if (sender is Hyperlink)
        {
            string uri = (sender as Hyperlink).Tag.ToString();

            if (ViewModel.HyperlinkCommand.CanExecute(uri))
                ViewModel.HyperlinkCommand.Execute(uri);
        }
    }

    private void ConvertToHyperlinkedText(TextBlock textBlock, string text)
    {
        foreach (Inline inline in textBlock.Inlines)
        {
            if (inline is Hyperlink)
            {
                (inline as Hyperlink).Click -= Hyperlink_Click;
            }
        }

        textBlock.Inlines.Clear();

        if (String.IsNullOrEmpty(text))
            return;

        List<Hyperlink> hyperlinks = new List<Hyperlink>();

        foreach (Match match in _hyperlinkCaptureRegex.Matches(text))
        {
            var hyperlink = new Hyperlink();

            hyperlink.Inlines.Add(match.Groups["text"].Value);
            hyperlink.Tag = match.Groups["link"].Value;
            hyperlink.Click += Hyperlink_Click;

            hyperlinks.Add(hyperlink);
        }

        string[] substrings = _hyperlinkRegex.Split(text);

        for (int i = 0; i < substrings.Length; i++)
        {
            textBlock.Inlines.Add(substrings[i]);

            if (i < hyperlinks.Count)
                textBlock.Inlines.Add(hyperlinks[i]);
        }
    }
}