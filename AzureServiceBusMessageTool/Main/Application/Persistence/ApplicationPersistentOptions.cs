using System.Collections.Generic;
using System.Linq;
using Main.Models;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;

namespace Main.Application.Persistence;

public class ApplicationPersistentOptions
{
   private readonly ServiceBusSelectedConfigsViewModel _serviceBusConfigsViewModel;
   private readonly SendersSelectedConfigViewModel _sendersSelectedConfigViewModel;
   private readonly ReceiversSelectedConfigViewModel _receiversSelectedConfigViewModel;

   public ApplicationPersistentOptions(
      ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel,
      SendersSelectedConfigViewModel sendersSelectedConfigViewModel,
      ReceiversSelectedConfigViewModel receiversSelectedConfigViewModel)
   {
      _serviceBusConfigsViewModel = serviceBusConfigsViewModel;
      _sendersSelectedConfigViewModel = sendersSelectedConfigViewModel;
      _receiversSelectedConfigViewModel = receiversSelectedConfigViewModel;
   }

   public List<ServiceBusConfigModel> GetServiceBusConfigsToStore()
   {
      return _serviceBusConfigsViewModel.ServiceBusConfigs.Select(e => e).ToList();
   }

   public void ReadServiceBusConfigsFrom(List<ServiceBusConfigModel> settingsServiceBusConfigs)
   {
      if (settingsServiceBusConfigs == null) return;
      _serviceBusConfigsViewModel.AddConfigs(settingsServiceBusConfigs);
   }

   public List<SenderConfigModel> GetSendersConfigsToStore()
   {
      return _sendersSelectedConfigViewModel.SendersConfigs.Select(e => e.Item).ToList();
   }

   public void ReadSendersConfigsFrom(List<SenderConfigModel> settingsSendersConfig)
   {
      if (settingsSendersConfig == null) return;
      _sendersSelectedConfigViewModel.AddConfigs(settingsSendersConfig);
   }

   public List<ReceiverConfigModel> GetReceiversConfigsToStore()
   {
      return _receiversSelectedConfigViewModel.ReceiversConfigs.Select(e => e.Item).ToList();
   }

   public void ReadReceiversConfigFrom(List<ReceiverConfigModel> settingsReceiversConfig)
   {
      if (settingsReceiversConfig == null) return;
      _receiversSelectedConfigViewModel.AddConfigs(settingsReceiversConfig);
   }
}
