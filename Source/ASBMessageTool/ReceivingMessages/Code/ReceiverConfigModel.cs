using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Model;
using JetBrains.Annotations;

namespace ASBMessageTool.ReceivingMessages.Code;

public sealed class ReceiverConfigModel : INotifyPropertyChanged
{
    private string _configName;
    private string _connectionString;
    private string _inputTopicName;
    private string _inputTopicSubscriptionName;
    private bool _isAttachedToDeadLetterSubqueue;
    private string _receiverQueueName;
    private bool _shouldScrollTextBoxToEndOnNewMessageReceive;
    private bool _shouldWordWrapLogContent;
    private bool _shouldShowOnlyMessageBodyAsJson;
    private int _msgBodyTextBoxFontSize = AppDefaults.DefaultTextBoxFontSize;
    private bool _shouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter;
    private IList<SBMessageApplicationProperty> _abandonMessageApplicationOverridenProperties = new ObservableCollection<SBMessageApplicationProperty>();
    private IList<SBMessageApplicationProperty> _deadLetterMessageApplicationOverridenProperties = new ObservableCollection<SBMessageApplicationProperty>();
    private SbDeadLetterMessageFields _sbDeadLetterMessageFields = new();
    private OnMessageReceiveEnumAction _onMessageReceiveAction;
    private DeadLetterMessageFieldsOverrideEnumType _deadLetterMessageFieldsOverrideType;
    private ReceiverDataSourceType _receiverDataSourceType;
    private bool _shouldReceiveOnlySelectedNumberOfMessages;
    private int _numberOfMessagesToReceive;


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
        get => _receiverQueueName;
        set
        {
            if (value == _receiverQueueName) return;
            _receiverQueueName = value;
            OnPropertyChanged();
        }
    }
    
        
    public int NumberOfMessagesToReceive
    {
        get => _numberOfMessagesToReceive;
        set
        {
            if (value == _numberOfMessagesToReceive) return;
            _numberOfMessagesToReceive = value;
            OnPropertyChanged();
        }
    }


    public OnMessageReceiveEnumAction OnMessageReceiveAction
    {
        get => _onMessageReceiveAction;
        set
        {
            if (value == _onMessageReceiveAction) return;
            _onMessageReceiveAction = value;
            OnPropertyChanged();
        }
    }

    public ReceiverDataSourceType ReceiverDataSourceType
    {
        get => _receiverDataSourceType;
        set
        {
            if (value == _receiverDataSourceType) return;
            _receiverDataSourceType = value;
            OnPropertyChanged();
        }
    }

    public DeadLetterMessageFieldsOverrideEnumType DeadLetterMessageFieldsOverrideType
    {
        get => _deadLetterMessageFieldsOverrideType;
        set
        {
            if (value == _deadLetterMessageFieldsOverrideType) return;
            _deadLetterMessageFieldsOverrideType = value;
            OnPropertyChanged();
        }
    }


    public SbDeadLetterMessageFields DeadLetterMessageFields
    {
        get => _sbDeadLetterMessageFields;
        set
        {
            if (value == _sbDeadLetterMessageFields) return;
            _sbDeadLetterMessageFields = value;
            OnPropertyChanged();
        }
    }

    public IList<SBMessageApplicationProperty> AbandonMessageApplicationOverridenProperties
    {
        get => _abandonMessageApplicationOverridenProperties;
        set
        {
            if (value == _abandonMessageApplicationOverridenProperties) return;
            _abandonMessageApplicationOverridenProperties = value;
            OnPropertyChanged();
        }
    }


    public IList<SBMessageApplicationProperty> DeadLetterMessageApplicationOverridenProperties
    {
        get => _deadLetterMessageApplicationOverridenProperties;
        set
        {
            if (value == _deadLetterMessageApplicationOverridenProperties) return;
            _deadLetterMessageApplicationOverridenProperties = value;
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
    
    
    public bool ShouldReceiveSpecificNumberOfMessages
    {
        get => _shouldReceiveOnlySelectedNumberOfMessages;
        set
        {
            if (value == _shouldReceiveOnlySelectedNumberOfMessages) return;
            _shouldReceiveOnlySelectedNumberOfMessages = value;
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

    [UsedImplicitly]
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

    [UsedImplicitly]
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
