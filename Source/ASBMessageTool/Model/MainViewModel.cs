using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.Application.Logging;

namespace ASBMessageTool.Model;

public sealed class MainViewModel : INotifyPropertyChanged, ILogContentAppender
{
    private readonly IAboutWindowProxy _aboutWindowProxy;
    private int _logTextBoxFontSize;
    private bool _shouldScrollToEndOnLogContentChange;
    private bool _shouldWordWrapLogContent = false;
    private string _logContent = string.Empty;
    private ServiceBusHelperLogger _logger;

    public MainViewModel(IAboutWindowProxy aboutWindowProxy)
    {
        _logger = new ServiceBusHelperLogger(this);
        _aboutWindowProxy = aboutWindowProxy;
        ShowAboutWindowCommand = new DelegateCommand(_ =>
        {
            _aboutWindowProxy.ShowWindow();
        });

        ClearLogsCommand = new DelegateCommand(_ =>
        {
            LogContent = string.Empty;
        });
    }

    public string LogContent
    {
        get => _logContent;
        set
        {
            if (value == _logContent) return;
            _logContent = value;
            OnPropertyChanged();
        }
    }

    public int LogTextBoxFontSize
    {
        get => _logTextBoxFontSize;
        set
        {
            if (value == _logTextBoxFontSize) return;
            _logTextBoxFontSize = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldWordWrapLogContent
    {
        get => _shouldWordWrapLogContent;
        set
        {
            if (value == _shouldWordWrapLogContent) return;
            _shouldWordWrapLogContent = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldScrollToEndOnLogContentChange
    {
        get => _shouldScrollToEndOnLogContentChange;
        set
        {
            if (value == _shouldScrollToEndOnLogContentChange) return;
            _shouldScrollToEndOnLogContentChange = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowAboutWindowCommand { get; }
    public ICommand ClearLogsCommand { get; }
    

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public IServiceBusHelperLogger GetLogger()
    {
        return _logger;
    }

    public void AddContent(string msg)
    {
        LogContent += msg;
    }
}
