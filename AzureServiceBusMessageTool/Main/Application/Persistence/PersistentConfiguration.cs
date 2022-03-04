using System.IO;
using nucs.JsonSettings;

namespace Main.Application.Persistence;

public class PersistentConfiguration
{
   private readonly ApplicationPersistentOptions _options;
   private string _configFilePath;
   private MySettings _settings;

   public PersistentConfiguration(ApplicationBinaryInfo binaryInfo, ApplicationPersistentOptions options )
   {
      _options = options;
      _configFilePath = $"{binaryInfo.ApplicationDirectory}{Path.DirectorySeparatorChar}{Globals.ConfigFileName}";

   }

   public void Load()
   {
      _settings = JsonSettings.Load<MySettings>(_configFilePath);
      _options.ReadServiceBusConfigsFrom(_settings.ServiceBusConfigs);
      _options.ReadSendersConfigsFrom(_settings.SendersConfig);
      _options.ReadReceiversConfigFrom(_settings.ReceiversConfig);
   }

   public void Save()
   {
      _settings.ReceiversConfig = _options.GetReceiversConfigsToStore();
      _settings.ServiceBusConfigs = _options.GetServiceBusConfigsToStore();
      _settings.SendersConfig = _options.GetSendersConfigsToStore();
      _settings.Save();
   }
}