namespace ASBMessageTool.SendingMessages;

public class SenderConfigModelFactory
{
    private int _staticCounter = 0;
    public SenderConfigModel Create()
    {
        return new SenderConfigModel()
        {
            ConfigName = $"sender configuration <{++_staticCounter}>"
        };
    }
}
