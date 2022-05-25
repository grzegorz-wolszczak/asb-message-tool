namespace ASBMessageTool.PeekingMessages.Code;

public class PeekerConfigModelFactory
{
    private static int _staticCounter = 0;
    public PeekerConfigModel Create()
    {
        return new PeekerConfigModel()
        {
            ConfigName = $"peeker configuration <{++_staticCounter}>"
        };
    }
}
