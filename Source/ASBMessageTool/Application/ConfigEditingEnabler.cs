using System;

namespace ASBMessageTool.Application;

public class ConfigEditingEnabler
{
    private readonly Action<bool> _setConfigEditingState;

    private bool _isExternalTaskBlockingEditingInProgress = false;
    private bool _validationInProgress = false;

    public ConfigEditingEnabler(Action<bool> setConfigEditingState)
    {
        _setConfigEditingState = setConfigEditingState;
    }

    public void SetConfigValidationFinished()
    {
        SetConfigValidationInProgress(false);
    }

    public void SetConfigValidationStarted()
    {
        SetConfigValidationInProgress(true);
    }

    private void SetConfigValidationInProgress(bool isInProgress)
    {
        _validationInProgress = isInProgress;
        EvaluateConfigEnabledState();
    }

    private void SetExternalTaskThatBlocksConfigurationEditingProgress(bool isInProgress)
    {
        _isExternalTaskBlockingEditingInProgress = isInProgress;
        EvaluateConfigEnabledState();
    }

    public void ExternalTaskThatBlocksConfigurationEditingStarted()
    {
        SetExternalTaskThatBlocksConfigurationEditingProgress(true);
    }

    public void ExternalTaskThatBlocksConfigurationEditingFinished()
    {
        SetExternalTaskThatBlocksConfigurationEditingProgress(false);
    }
        
    private void EvaluateConfigEnabledState()
    {
        if (_isExternalTaskBlockingEditingInProgress == false && _validationInProgress == false)
        {
            _setConfigEditingState.Invoke(true);
            return;
        }
        _setConfigEditingState.Invoke(false);
    }
}
