namespace Main.ViewModels.Configs.Receivers
{
   public interface IServiceBusMessageReceiver
   {
      void Start(ServiceBusReceiverSettings config, ReceiverCallbacks callbacks);
      void Stop();

   }
}
