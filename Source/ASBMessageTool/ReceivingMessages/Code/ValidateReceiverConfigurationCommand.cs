using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.ReceivingMessages.Code;

public class ValidateReceiverConfigurationCommand : ICommand
{
    private readonly Action _onValidationStartedAction;
    private readonly Action _onValidationFinishedAction;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly Func<ServiceBusReceiverSettings> _getReceiveDataFunction;
    private readonly IReceiverSettingsValidator _settingsValidator;
    private bool _canExecute = true;

    public ValidateReceiverConfigurationCommand(
        Action onValidationStartedAction,
        Action onValidationFinishedAction,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        Func<ServiceBusReceiverSettings>  getReceiveDataFunction,
        IReceiverSettingsValidator settingsValidator)
    {
        _onValidationStartedAction = onValidationStartedAction;
        _onValidationFinishedAction = onValidationFinishedAction;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _getReceiveDataFunction = getReceiveDataFunction;
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
                var dataToValidate = _getReceiveDataFunction();
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
                            $"Configuration for receiver config '{dataToValidate.ConfigName}' is correct.");
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
