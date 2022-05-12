namespace ASBMessageTool.ReceivingMessages;

public class ReceiversConfigModelFactory
{
    private static int _staticCounter = 0;

    public ReceiverConfigModel Create()
    {
        var item = new ReceiverConfigModel()
        {
            ConfigName = $"receiver configuration <{++_staticCounter}>"
        };
        return item;
    }
}
