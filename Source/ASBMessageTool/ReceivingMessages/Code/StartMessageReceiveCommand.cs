using System;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.ReceivingMessages.Code;

public class StartMessageReceiveCommand : ICommand
{
    private readonly IServiceBusMessageReceiver _msgReceiver;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly Func<ServiceBusReceiverSettings> _serviceBusReceiverProviderFunc;
    private readonly Action<ReceivedMessage> _onMessageReceived;
    private readonly Action _onReceiverStarted;
    private readonly Action<Exception> _onReceiverFailure;
    private readonly Action _onReceiverStopped;
    private readonly Action _onReceiverInitializing;
    private readonly Action<string> _onOutputFromReceiverReceived;

    private bool _canExecute = true;

    public StartMessageReceiveCommand(IServiceBusMessageReceiver msgReceiver,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        Func<ServiceBusReceiverSettings> serviceBusReceiverProviderFunc,
        Action<ReceivedMessage> onMessageReceived,
        Action onReceiverStarted,
        Action<Exception> onReceiverFailure,
        Action onReceiverStopped,
        Action onReceiverInitializing,
        Action<string> onOutputFromReceiverReceived)
    {
        _msgReceiver = msgReceiver;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _serviceBusReceiverProviderFunc = serviceBusReceiverProviderFunc;
        _onMessageReceived = onMessageReceived;
        _onReceiverStarted = onReceiverStarted;
        _onReceiverFailure = onReceiverFailure;
        _onReceiverStopped = onReceiverStopped;
        _onReceiverInitializing = onReceiverInitializing;
        _onOutputFromReceiverReceived = onOutputFromReceiverReceived;
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
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                _onReceiverFailure.Invoke(exc);
            },
            OnOutputFromReceiverReceived = _onOutputFromReceiverReceived,
            OnMessageReceive = _onMessageReceived,
            OnReceiverStop = ()=>
            {
                _canExecute = true;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                _onReceiverStopped.Invoke();
            },
            OnReceiverStarted = _onReceiverStarted,
            OnReceiverInitializing = _onReceiverInitializing,
        };
        _canExecute = false;
        _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
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
