namespace ASBMessageTool.ReceivingMessages.Code;

public class MessagePropertiesWindowProxyFactory
{
    public IMessageApplicationPropertiesWindowProxy Create()
    {
        return new MessageApplicationPropertiesWindowProxy();
    }
}
