using Main.Windows.ApplicationProperties;

namespace Main.ViewModels.Configs.Receivers;

public class MessagePropertiesWindowProxyFactory
{
    public IMessageApplicationPropertiesWindowProxy Create()
    {
        return new MessageApplicationPropertiesWindowProxy();
    }
}
