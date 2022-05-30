using System;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.SendingMessages.Code;

public class StopSendingMessagesCommand : ICommand
{
    private readonly IMessageSender _messageSender;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private bool _canExecute = false;
    private bool _isSendingInProgress = false;
    private bool _isStoppingInProgress = false;

    public StopSendingMessagesCommand(IMessageSender messageSender, IInGuiThreadActionCaller inGuiThreadActionCaller)
    {
        _messageSender = messageSender;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
    }
    
    public bool CanExecute(object parameter)
    {
        return _canExecute;
    }

    public void Execute(object parameter)
    {
        _messageSender.Stop();
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void IndicateSendingStopping()
    {
        _isStoppingInProgress = true;
        _isSendingInProgress = false;
        RecalculateCanExecute();
    }

    public void IndicateSendingStopped()
    {
        _isStoppingInProgress = false;
        RecalculateCanExecute();
    }

    public void OnStartSending()
    {
        _isSendingInProgress = true;
        _isStoppingInProgress = false;
        RecalculateCanExecute();
    }

    public void OnSendingFinished()
    {
        _isSendingInProgress = false;
        _isStoppingInProgress = false;
        RecalculateCanExecute();
    }

    public void OnSendingFailed()
    {
        _isSendingInProgress = false;
        _isStoppingInProgress = false;
        RecalculateCanExecute();
    }

    private void RecalculateCanExecute()
    { _canExecute = (_isSendingInProgress && !_isStoppingInProgress);
        
        _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
    }
}

