using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.Model;

namespace ASBMessageTool.ReceivingMessages;

public class DeadLetterMessagePropertiesViewModel : INotifyPropertyChanged, IServiceBusMessageApplicationPropertiesHolder
{
    private readonly IList<SBMessageApplicationProperty> _messageApplicationProperties;
    private readonly SbDeadLetterMessageFields _itemDeadLetterMessageFields;
    private readonly ReceiverConfigModel _item;

    private SBMessageApplicationProperty _selectedItem;

    public DeadLetterMessagePropertiesViewModel(ReceiverConfigModel item)
    {
        _item = item;
        _messageApplicationProperties = _item.DeadLetterMessageApplicationOverridenProperties;
        _itemDeadLetterMessageFields = _item.DeadLetterMessageFields;


        AddMessagePropertyCommand = new DelegateCommand(_ =>
        {
            _messageApplicationProperties.Add(new SBMessageApplicationProperty
            {
                PropertyName = "",
                PropertyValue = ""
            });
        });
        DeleteMessagePropertyCommand = new DelegateCommand(_ =>
        {
            if (SelectedItem != null)
            {
                _messageApplicationProperties.Remove(SelectedItem);
            }
        },_=> SelectedItem != null);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SbDeadLetterMessageFields DeadLetterMessageFields
    {
        get => _itemDeadLetterMessageFields;
    }

    public IList<SBMessageApplicationProperty> ApplicationProperties
    {
        get => _messageApplicationProperties;
    }

    public DeadLetterMessageFieldsOverrideEnumType DeadLetterMessageFieldsOverrideType
    {
        get => _item.DeadLetterMessageFieldsOverrideType;
        set
        {
            if (value == _item.DeadLetterMessageFieldsOverrideType) return;
            _item.DeadLetterMessageFieldsOverrideType = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddMessagePropertyCommand { get; }
    public ICommand DeleteMessagePropertyCommand { get; }


    public SBMessageApplicationProperty SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (value == _selectedItem) return;
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    public IList<string> GetDuplicatedApplicationProperties()
    {
        return _messageApplicationProperties.GroupBy(x => x.PropertyName)
            .Where(g => g.Count() > 1)
            .Select(x => x.Key)
            .ToList();
    }

    public void RemoveEmptyProperties()
    {
        var itemsToRemove = _messageApplicationProperties.Where(e => string.IsNullOrWhiteSpace(e.PropertyName)).ToList();
        foreach (var itemToRemove in itemsToRemove)
        {
            _messageApplicationProperties.Remove(itemToRemove);
        }
    }
}
