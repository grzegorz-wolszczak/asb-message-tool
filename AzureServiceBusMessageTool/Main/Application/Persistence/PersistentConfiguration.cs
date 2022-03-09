using System.IO;
using Main.Application.Logging;
using nucs.JsonSettings;

namespace Main.Application.Persistence
{
   public class PersistentConfiguration
   {
      private readonly IServiceBusHelperLogger _logger;
      private readonly ApplicationPersistentOptions _options;
      private string _configFilePath;
      private MySettings _settings;

      public PersistentConfiguration(IServiceBusHelperLogger logger, ApplicationBinaryInfo binaryInfo,
         ApplicationPersistentOptions options)
      {
         _logger = logger;
         _options = options;
         _configFilePath = $"{binaryInfo.ApplicationDirectory}{Path.DirectorySeparatorChar}{StaticConfig.ConfigFileName}";

      }

      public void Load()
      {
         _logger.LogInfo($"Reading config from file '{_configFilePath}'");
         _settings = JsonSettings.Load<MySettings>(_configFilePath);
         _options.ReadServiceBusConfigsFrom(_settings.ServiceBusConfigs);
         _options.ReadSendersConfigsFrom(_settings.SendersConfig);
         _options.ReadReceiversConfigFrom(_settings.ReceiversConfig);
         _options.ReadMainWindowSettings(_settings.MainWindowSettings);
      }

      public void Save()
      {
         _settings.ReceiversConfig = _options.GetReceiversConfigsToStore();
         _settings.ServiceBusConfigs = _options.GetServiceBusConfigsToStore();
         _settings.SendersConfig = _options.GetSendersConfigsToStore();
         _settings.MainWindowSettings = _options.GetMainWindowSettings();
         _settings.Save();
      }
   }
}