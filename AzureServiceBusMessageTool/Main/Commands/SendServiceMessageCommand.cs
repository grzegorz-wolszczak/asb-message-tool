using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Main.ViewModels.Configs.Senders;

namespace Main.Commands;

public class SendServiceMessageCommand : ICommand
{
    private readonly IMessageSender _messageSender;
    private readonly Func<ServiceBusMessageSendData> _msgProviderFunc;
    private readonly Action<MessageSendErrorInfo> _onErrorAction;
    private readonly Action<string> _onSuccessAction;
    private readonly Action<Exception> _onUnexpectedExceptionAction;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private bool _canExecute = true;

    public SendServiceMessageCommand(IMessageSender messageSender,
        Func<ServiceBusMessageSendData> msgProviderFunc,
        Action<MessageSendErrorInfo> onErrorAction,
        Action<string> onSuccessAction,
        Action<Exception> onUnexpectedExceptionAction,
        IInGuiThreadActionCaller inGuiThreadActionCaller)
    {
        _messageSender = messageSender;
        _msgProviderFunc = msgProviderFunc;
        _onErrorAction = onErrorAction;
        _onSuccessAction = onSuccessAction;
        _onUnexpectedExceptionAction = onUnexpectedExceptionAction;
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
                _canExecute = false;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                ServiceBusMessageSendData serviceBusMessageSendData = _msgProviderFunc.Invoke();
                var maybeError = await _messageSender.Send(serviceBusMessageSendData);
                if (maybeError.HasValue)
                {
                    _onErrorAction(maybeError.Value());
                }
                else
                {
                    _onSuccessAction("Message sent successfully");
                }
            }
            catch (Exception e)
            {
                _onUnexpectedExceptionAction(e);
            }
            finally
            {
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
