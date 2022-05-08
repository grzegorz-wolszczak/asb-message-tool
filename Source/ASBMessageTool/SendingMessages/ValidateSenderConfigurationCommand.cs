using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.SendingMessages;

public class ValidateSenderConfigurationCommand : ICommand
{
    private readonly Action _onValidationStartedAction;
    private readonly Action _onValidationFinishedAction;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly Func<ServiceBusMessageSendData> _getSendDataFunction;
    private readonly ISenderSettingsValidator _senderSettingsValidator;
    private bool _canExecute = true;

    public ValidateSenderConfigurationCommand(
        Action onValidationStartedAction,
        Action onValidationFinishedAction,
        IInGuiThreadActionCaller inGuiThreadActionCaller, 
        Func<ServiceBusMessageSendData> getSendDataFunction,
        ISenderSettingsValidator senderSettingsValidator)
    {
        _onValidationStartedAction = onValidationStartedAction;
        _onValidationFinishedAction = onValidationFinishedAction;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _getSendDataFunction = getSendDataFunction;
        _senderSettingsValidator = senderSettingsValidator;
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
                _onValidationStartedAction.Invoke();
                _canExecute = false;
                _inGuiThreadActionCaller.Call(CommandManager.InvalidateRequerySuggested);
                var sendData = _getSendDataFunction();
                var validationResult = await _senderSettingsValidator.Validate(sendData, default);
                if (validationResult.HasValue)
                {
                    _inGuiThreadActionCaller.Call(() =>
                    {
                        UserInteractions.ShowErrorDialog("Invalid configuration", validationResult.Value().ErrorMsg);
                    });
                }
            }
            finally
            {
                _canExecute = true;
                _onValidationFinishedAction.Invoke();
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


/*
        // ValidateConfigurationCommand = new DelegateCommand(_ =>
        // {
        //     Task.Factory.StartNew(async () =>
        //     {
        //         // disable command
        //         try
        //         {
        //             var sendData = GetServiceBusMessageSendData();
        //             var validationResult = await _senderSettingsValidator.Validate(sendData, default);
        //             if (validationResult.HasValue)
        //             {
        //                 _inGuiThreadActionCaller.Call(() =>
        //                 {
        //                     UserInteractions.ShowErrorDialog("Invalid configuration", validationResult.Value().ErrorMsg);
        //                 });
        //             }
        //         }
        //         finally
        //         {
        //             // enable command
        //         }
        //         
        //     }, TaskCreationOptions.LongRunning);
        // });
        */
