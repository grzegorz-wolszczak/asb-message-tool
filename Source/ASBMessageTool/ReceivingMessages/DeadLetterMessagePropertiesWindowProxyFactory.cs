namespace ASBMessageTool.ReceivingMessages;

public class DeadLetterMessagePropertiesWindowProxyFactory
{
    public IDeadLetterMessagePropertiesWindowProxy Create()
    {
        return new DeadLetterMessagePropertiesWindowProxy();
    }
}