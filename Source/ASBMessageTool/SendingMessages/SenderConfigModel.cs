using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Model;

namespace ASBMessageTool.SendingMessages;

public class SenderConfigModel : INotifyPropertyChanged
{
    private string _configName = string.Empty;
    private SbMessageStandardFields _sbMessageStandardFields = new();
    private IList<SBMessageApplicationProperty> _messageApplicationProperties = new ObservableCollection<SBMessageApplicationProperty>();
    private string _serviceBusConnectionString = string.Empty;
    private string _outputTopicName = string.Empty;
    private string _body = string.Empty;
    private int _msgBodyTextFontSize = AppDefaults.DefaultTextBoxFontSize;

    public string ConfigName
    {
        get => _configName;
        set
        {
            if (value == _configName) return;
            _configName = value;
            OnPropertyChanged();
        }
    }


    public string MsgBody
    {
        get => _body;
        set
        {
            if (value == _body) return;
            _body = value;
            OnPropertyChanged();
        }
    }

    public int MsgBodyTextBoxFontSize
    {
        get => _msgBodyTextFontSize;
        set
        {
            if (value == _msgBodyTextFontSize) return;
            _msgBodyTextFontSize = value;
            OnPropertyChanged();
        }
    }

    public string ServiceBusConnectionString
    {
        get => _serviceBusConnectionString;
        set
        {
            if (value == _serviceBusConnectionString) return;
            _serviceBusConnectionString = value;
            OnPropertyChanged();
        }
    }

    public string OutputTopicName
    {
        get => _outputTopicName;
        set
        {
            if (value == _outputTopicName) return;
            _outputTopicName = value;
            OnPropertyChanged();
        }
    }


    public SbMessageStandardFields MessageFields
    {
        get => _sbMessageStandardFields;
        set
        {
            if (value == _sbMessageStandardFields) return;
            _sbMessageStandardFields = value;
            OnPropertyChanged();
        }
    }

    public IList<SBMessageApplicationProperty> ApplicationProperties
    {
        get => _messageApplicationProperties;
        set
        {
            if (value == _messageApplicationProperties) return;
            _messageApplicationProperties = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
