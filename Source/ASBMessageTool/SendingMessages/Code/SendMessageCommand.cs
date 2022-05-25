using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.SendingMessages.Code;

public class SendMessageCommand : ICommand
{
    private readonly IMessageSender _messageSender;
    private readonly Action _onStartSendingAction;
    private readonly Action _onFinishedSendingAction;
    private readonly Func<ServiceBusMessageSendData> _msgProviderFunc;
    private readonly Action<MessageSendErrorInfo> _onErrorWhileSendingHappenedAction;
    private readonly Action<Exception> _onSendingErrorHappened;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private bool _canExecute = true;

    public SendMessageCommand(IMessageSender messageSender,
        Action onStartSendingAction,
        Action onFinishedSendingAction,
        Func<ServiceBusMessageSendData> msgProviderFunc,
        Action<MessageSendErrorInfo> onErrorWhileSendingHappenedAction,
        Action<Exception> onSendingErrorHappened,
        IInGuiThreadActionCaller inGuiThreadActionCaller)
    {
        _messageSender = messageSender;
        _onStartSendingAction = onStartSendingAction;
        _onFinishedSendingAction = onFinishedSendingAction;
        _msgProviderFunc = msgProviderFunc;
        _onErrorWhileSendingHappenedAction = onErrorWhileSendingHappenedAction;
        _onSendingErrorHappened = onSendingErrorHappened;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute;
    }

    public void Execute(object parameter)
    {
        Task.Factory.StartNew(async () =>
        {
            try
            {
                _onStartSendingAction.Invoke();
                _canExecute = false;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                ServiceBusMessageSendData serviceBusMessageSendData = _msgProviderFunc.Invoke();
                var maybeError = await _messageSender.Send(serviceBusMessageSendData);
                if (maybeError.HasValue)
                {
                    _onErrorWhileSendingHappenedAction(maybeError.Value());
                }
            }
            catch (Exception e)
            {
                _onSendingErrorHappened(e);
            }
            finally
            {
                _onFinishedSendingAction.Invoke();
                _canExecute = true;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
            }
        },TaskCreationOptions.LongRunning);
    }


    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
