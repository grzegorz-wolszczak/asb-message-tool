using ASBMessageTool.Application.Logging;
using nucs.JsonSettings;

namespace ASBMessageTool.Application.Persistence;

public class PersistentConfiguration
{
    private readonly ApplicationPersistentOptions _options;
    private readonly IServiceBusHelperLogger _logger;
    private readonly string _configFilePath;
    private readonly AsbToolPersistentSettings _settings;


    public PersistentConfiguration(
        string configFilePath,
        ApplicationPersistentOptions options,
        IServiceBusHelperLogger logger)
    {
        _options = options;
        _logger = logger;
        _configFilePath = configFilePath;
        _settings = JsonSettings.Configure<AsbToolPersistentSettings>(_configFilePath);
    }

    // todo: add handling if loading from config file fails
    // suggest : loading empty file (will be overwritten when closing app)
    // : ingore application open and fix file manually
    public void Load()
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
