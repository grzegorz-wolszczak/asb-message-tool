using System.Collections.Generic;
using Main.Models;
using nucs.JsonSettings;

namespace Main.Application.Persistence;

public class MySettings : JsonSettings
{
   public override string FileName { get; set; }

   public MainWindowSettings MainWindowSettings {
      get;
      set;
   }
   public List<ServiceBusConfigModel> ServiceBusConfigs { get; set; }
   public List<SenderConfigModel> SendersConfig { get; set; }
   public List<ReceiverConfigModel> ReceiversConfig { get; set; }
}

public class MainWindowSettings
{
   public bool ShouldScrollToEndOnLogContentChange { get; set; }
   public bool ShouldWordWrapLogContent { get; set; }
}
