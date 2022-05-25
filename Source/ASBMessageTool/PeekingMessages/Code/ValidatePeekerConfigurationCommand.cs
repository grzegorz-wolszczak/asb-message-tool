using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.PeekingMessages.Code;

public class ValidatePeekerConfigurationCommand : ICommand
{
    private readonly Action _onValidationStartedAction;
    private readonly Action _onValidationFinishedAction;
    private readonly InGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly Func<ServiceBusPeekerSettings> _getServiceBusReceiverSettings;
    private readonly IPeekerSettingsValidator _settingsValidator;
    private bool _canExecute = true;

    public ValidatePeekerConfigurationCommand(Action onValidationStartedAction, 
        Action onValidationFinishedAction, 
        InGuiThreadActionCaller inGuiThreadActionCaller, 
        Func<ServiceBusPeekerSettings> getServiceBusReceiverSettings, 
        IPeekerSettingsValidator settingsValidator)
    {
        _onValidationStartedAction = onValidationStartedAction;
        _onValidationFinishedAction = onValidationFinishedAction;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _getServiceBusReceiverSettings = getServiceBusReceiverSettings;
        _settingsValidator = settingsValidator;
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
                var dataToValidate = _getServiceBusReceiverSettings();
                var validationResult = await _settingsValidator.Validate(dataToValidate, default);
                if (validationResult.HasValue)
                {
                    _inGuiThreadActionCaller.Call(() =>
                    {
                        UserInteractions.ShowErrorDialog("Invalid configuration", validationResult.Value().ErrorMsg);
                    });
                }
                else
                {
                    _inGuiThreadActionCaller.Call(() =>
                    {
                        UserInteractions.ShowInformationDialog("Configuration validation",
                            $"Configuration for peeker config '{dataToValidate.ConfigName}' is correct.");
                    });
                }
            }
            catch (Exception exception)
            {
                _inGuiThreadActionCaller.Call(() => { UserInteractions.ShowExceptionDialog("Exception during validation", exception); });
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
