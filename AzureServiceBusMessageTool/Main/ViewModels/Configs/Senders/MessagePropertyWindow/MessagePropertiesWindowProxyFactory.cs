namespace Main.ViewModels.Configs.Senders.MessagePropertyWindow;

public class MessagePropertiesWindowProxyFactory
{
   public IMessagePropertiesWindowProxy Create()
   {
      return new MessagePropertiesWindowProxy();
   }
}
