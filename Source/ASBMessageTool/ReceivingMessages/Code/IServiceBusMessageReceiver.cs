namespace ASBMessageTool.ReceivingMessages.Code;

public interface IServiceBusMessageReceiver
{
    void Start(ServiceBusReceiverSettings config, ReceiverCallbacks callbacks);
    void Stop();

}
