using System;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Application.Logging;
using nucs.JsonSettings;

namespace ASBMessageTool.Application.Persistence;

public class PersistentConfiguration
{
    private readonly ApplicationPersistentOptions _options;
    private readonly IServiceBusHelperLogger _logger;
    private readonly ApplicationShutdowner _applicationShutdowner;
    private readonly string _configFilePath;
    private readonly AsbToolPersistentSettings _settings;


    public PersistentConfiguration(string configFilePath,
        ApplicationPersistentOptions options,
        IServiceBusHelperLogger logger, 
        ApplicationShutdowner applicationShutdowner)
    {
        _options = options;
        _logger = logger;
        _applicationShutdowner = applicationShutdowner;
        _configFilePath = configFilePath;
        _settings = JsonSettings.Configure<AsbToolPersistentSettings>(_configFilePath);
    }
    
    public void Load()
    {
        try
        {
            TryLoad();
        }
        catch (Exception e)
        {
            HandleException(e);
        }
    }

    private void HandleException(Exception exception)
    {
        var message = $"During reading configuration file '{_configFilePath}'\n\nexception happened:\n\n{exception.Message}\n\n" +
                      $"Do you want to open the application ?\n\n" +
                      $"If you choose YES application will ERASE ALL DATA in config file\n" +
                      $"If you choose NO application will shutdown and you will have the chance to fix the config file manually";
        var answer = UserInteractions.ShowYesNoQueryDialog(
            "Configuration file is invalid.", 
            message, 
            $"{StaticConfig.ApplicationName} opening error",
            exception);
        if (answer == UserInteractions.YesNoDialogResult.No)
        {
            _applicationShutdowner.Shutdown();
        }
        
    }

    private void TryLoad()
    {
        _logger.LogInfo($"Reading config from file '{_configFilePath}'");
        _settings.Load();
        _options.ReadMainWindowSettings(_settings.MainWindowSettings);
        _options.ReadSendersConfigSettings(_settings.SendersConfig);
        _options.ReadReceiversConfigSettings(_settings.ReceiversConfig);
    }

    public void Save()
    {
        _settings.MainWindowSettings = _options.GetMainWindowSettings();
        _settings.SendersConfig = _options.GetSendersConfigToStore();
        _settings.ReceiversConfig = _options.GetReceiversConfigToStore();
        _settings.Save();
    }
}
