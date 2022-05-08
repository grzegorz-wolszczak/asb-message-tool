namespace ASBMessageTool.ReceivingMessages;

public interface IServiceBusMessageReceiver
{
    void Start(ServiceBusReceiverSettings config, ReceiverCallbacks callbacks);
    void Stop();

}
