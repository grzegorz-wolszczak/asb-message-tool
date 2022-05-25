namespace ASBMessageTool.ReceivingMessages.Code;

public class DeadLetterMessagePropertiesWindowProxyFactory
{
    public IDeadLetterMessagePropertiesWindowProxy Create()
    {
        return new DeadLetterMessagePropertiesWindowProxy();
    }
}