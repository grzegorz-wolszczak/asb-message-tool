using System;
using System.Windows.Input;

namespace ASBMessageTool.ReceivingMessages;

public class StartMessageReceiveCommand : ICommand
{
    private readonly IServiceBusMessageReceiver _msgReceiver;
    private readonly Func<ServiceBusReceiverSettings> _serviceBusReceiverProviderFunc;
    private readonly Action<ReceivedMessage> _onMessageReceived;
    private readonly Action _onReceiverStarted;
    private readonly Action<Exception> _onReceiverFailure;
    private readonly Action _onReceiverStopped;
    private readonly Action _onReceiverInitializing;

    private bool _canExecute = true;

    public StartMessageReceiveCommand(
        IServiceBusMessageReceiver msgReceiver,
        Func<ServiceBusReceiverSettings> serviceBusReceiverProviderFunc,
        Action<ReceivedMessage> onMessageReceived,
        Action onReceiverStarted,
        Action<Exception> onReceiverFailure,
        Action onReceiverStopped,
        Action onReceiverInitializing)
    {
        _msgReceiver = msgReceiver;
        _serviceBusReceiverProviderFunc = serviceBusReceiverProviderFunc;
        _onMessageReceived = onMessageReceived;
        _onReceiverStarted = onReceiverStarted;
        _onReceiverFailure = onReceiverFailure;
        _onReceiverStopped = onReceiverStopped;
        _onReceiverInitializing = onReceiverInitializing;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute;
    }

    public void Execute(object parameter)
    {
        var callbacks = new ReceiverCallbacks
        {
            OnReceiverFailure = exc=>
            {
                _canExecute = true;
                // todo: replace this call with inGuiThreadCaller or someting
                System.Windows.Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
                _onReceiverFailure.Invoke(exc);
            },
            OnMessageReceive = _onMessageReceived,
            OnReceiverStop = ()=>
            {
                _canExecute = true;
                System.Windows.Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
                _onReceiverStopped.Invoke();
            },
            OnReceiverStarted = _onReceiverStarted,
            OnReceiverInitializing = _onReceiverInitializing,
        };
        _canExecute = false;
        System.Windows.Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        _msgReceiver.Start(
            _serviceBusReceiverProviderFunc.Invoke(),
            callbacks
        );
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
