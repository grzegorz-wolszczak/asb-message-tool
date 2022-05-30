using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekMessagesCommand : ICommand
{
    private readonly IServiceBusMessagePeeker _msgPeeker;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly Func<ServiceBusPeekerSettings> _serviceBusPeekerSettingsProviderFunc;
    private readonly Action<List<PeekedMessage>> _onAllMessagesPeeked;
    private readonly Action _onPeekerStarted;
    private readonly Action<Exception> _onPeekerFailure;
    private readonly Action _onPeekerStopped;
    private readonly Action _onPeekerInitializing;
    private bool _canExecute = true;

    public PeekMessagesCommand(IServiceBusMessagePeeker msgPeeker,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        Func<ServiceBusPeekerSettings> serviceBusPeekerSettingsProviderFunc,
        Action<List<PeekedMessage>> onAllMessagesPeeked,
        Action onPeekerStarted,
        Action<Exception> onPeekerFailure,
        Action onPeekerStopped,
        Action onPeekerInitializing)
    {
        _msgPeeker = msgPeeker;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _serviceBusPeekerSettingsProviderFunc = serviceBusPeekerSettingsProviderFunc;
        _onAllMessagesPeeked = onAllMessagesPeeked;
        _onPeekerStarted = onPeekerStarted;
        _onPeekerFailure = onPeekerFailure;
        _onPeekerStopped = onPeekerStopped;
        _onPeekerInitializing = onPeekerInitializing;
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
                
                var callbacks = new PeekerCallbacks
                {
                    OnPeekerFailure = exc =>
                    {
                        _canExecute = true;
                        _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                        _onPeekerFailure.Invoke(exc);
                    },
                    OnAllMessagesPeeked = _onAllMessagesPeeked,
                    OnPeekerFinished = () =>
                    {
                        _canExecute = true;
                        _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                        _onPeekerStopped.Invoke();
                    },
                    OnPeekerStarted = _onPeekerStarted,
                    OnPeekerInitializing = _onPeekerInitializing
                };

                await _msgPeeker.Start(_serviceBusPeekerSettingsProviderFunc.Invoke(), callbacks);
            }
            finally
            {
                _canExecute = true;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
            }
            
        }, TaskCreationOptions.LongRunning);

        
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
