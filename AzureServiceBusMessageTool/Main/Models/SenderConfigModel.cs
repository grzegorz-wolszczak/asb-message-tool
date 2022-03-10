using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Main.Application;
using Main.ViewModels.Configs.Senders;
using Main.ViewModels.Configs.Senders.MessagePropertyWindow;

namespace Main.Models;

public sealed class SenderConfigModel : INotifyPropertyChanged
{
    private SbMessageStandardFields _sbMessageStandardFields = new();
    private IList<SBMessageApplicationProperty> _messageApplicationProperties = new ObservableCollection<SBMessageApplicationProperty>();
    private string _configName = string.Empty;
    private string _serviceBusConnectionString = string.Empty;
    private string _outputTopicName = string.Empty;
    private int _msgBodyTextFontSize = AppDefaults.DefaultTextBoxFontSize;
    private string _body = string.Empty;

    public string ConfigId { get; init; }

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

    public event PropertyChangedEventHandler PropertyChanged;

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

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}