using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.SendingMessages.Code;

public class SendMessageCommand : ICommand
{
    private readonly IMessageSender _messageSender;
    private readonly Func<ServiceBusMessageSendData> _msgProviderFunc;
    private readonly Action<Exception> _onUnexpectedErrorHappened;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private bool _canExecute = true;
    private readonly SenderCallbacks _senderCallbacks;

    public SendMessageCommand(IMessageSender messageSender,
        Func<ServiceBusMessageSendData> msgProviderFunc,
        Action<Exception> onUnexpectedErrorHappened,
        IInGuiThreadActionCaller inGuiThreadActionCaller, SenderCallbacks senderCallbacks)
    {
        _messageSender = messageSender;
        _msgProviderFunc = msgProviderFunc;
        _onUnexpectedErrorHappened = onUnexpectedErrorHappened;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _senderCallbacks = senderCallbacks;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute;
    }

    public void Execute(object parameter)
    {
        var task = Task.Factory.StartNew(async () =>
        {
            try
            {
                _canExecute = false;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                await _messageSender.Send(_senderCallbacks, _msgProviderFunc.Invoke());
            }
            catch (Exception e)
            {
                _onUnexpectedErrorHappened(e);
            }
            finally
            {
                _canExecute = true;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
            }
        }, TaskCreationOptions.LongRunning);

        task.ContinueWith((t) => { _onUnexpectedErrorHappened(t.Exception); },
            TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
    }


    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
