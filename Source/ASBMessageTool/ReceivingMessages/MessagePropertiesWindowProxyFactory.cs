namespace ASBMessageTool.ReceivingMessages;

public class MessagePropertiesWindowProxyFactory
{
    public IMessageApplicationPropertiesWindowProxy Create()
    {
        return new MessageApplicationPropertiesWindowProxy();
    }
}
