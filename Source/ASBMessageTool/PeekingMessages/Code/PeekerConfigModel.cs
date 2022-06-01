using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Model;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekerConfigModel : INotifyPropertyChanged
{
    private string _configName ;
    private string _connectionString;
    private string _inputTopicName;
    private string _inputTopicSubscriptionName;
    private bool _isAttachedToDeadLetterSubqueue;
    private string _peekerQueueName;
    private bool _shouldScrollTextBoxToEndOnNewMessageReceive;
    private bool _shouldWordWrapLogContent;
    private bool _shouldShowOnlyMessageBodyAsJson;
    private int _msgBodyTextBoxFontSize = AppDefaults.DefaultTextBoxFontSize;
    private bool _shouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter;
    private ReceiverDataSourceType _peekerDataSourceType;

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

    public string ReceiverQueueName
    {
        get => _peekerQueueName;
        set
        {
            if (value == _peekerQueueName) return;
            _peekerQueueName = value;
            OnPropertyChanged();
        }
    }

    public string ServiceBusConnectionString
    {
        get => _connectionString;
        set
        {
            if (value == _connectionString) return;
            _connectionString = value;
            OnPropertyChanged();
        }
    }

    public ReceiverDataSourceType ReceiverDataSourceType
    {
        get => _peekerDataSourceType;
        set
        {
            if (value == _peekerDataSourceType) return;
            _peekerDataSourceType = value;
            OnPropertyChanged();
        }
    }
    public int MsgBodyTextBoxFontSize
    {
        get => _msgBodyTextBoxFontSize;
        set
        {
            if (value == _msgBodyTextBoxFontSize) return;
            _msgBodyTextBoxFontSize = value;
            OnPropertyChanged();
        }
    }

    
    public bool IsAttachedToDeadLetterSubqueue
    {
        get => _isAttachedToDeadLetterSubqueue;
        set
        {
            if (value == _isAttachedToDeadLetterSubqueue) return;
            _isAttachedToDeadLetterSubqueue = value;
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
    
    public bool ShouldShowOnlyBodyAsJson
    {
        get => _shouldShowOnlyMessageBodyAsJson;
        set
        {
            if (value == _shouldShowOnlyMessageBodyAsJson) return;
            _shouldShowOnlyMessageBodyAsJson = value;
            OnPropertyChanged();
        }
    }

    
    
    public string InputTopicName
    {
        get => _inputTopicName;
        set
        {
            if (value == _inputTopicName) return;
            _inputTopicName = value;
            OnPropertyChanged();
        }
    }

    public string InputTopicSubscriptionName
    {
        get => _inputTopicSubscriptionName;
        set
        {
            if (value == _inputTopicSubscriptionName) return;
            _inputTopicSubscriptionName = value;
            OnPropertyChanged();
        }
    }
    
    
    public bool ShouldScrollTextBoxToEndOnNewMessageReceive
    {
        get => _shouldScrollTextBoxToEndOnNewMessageReceive;
        set
        {
            if (value == _shouldScrollTextBoxToEndOnNewMessageReceive) return;
            _shouldScrollTextBoxToEndOnNewMessageReceive = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter
    {
        get => _shouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter;
        set
        {
            if (value == _shouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter) return;
            _shouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter = value;
            OnPropertyChanged();
        }
    }
    
    public string ConfigId { get; init; } // UUID
    
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
