using Main.Windows.MessageToSend;

namespace Main.ViewModels.Configs.Senders.MessagePropertyWindow;

public class SenderMessagePropertiesWindowProxyFactory
{
   public ISenderMessagePropertiesWindowProxy Create()
   {
      return new SenderMessagePropertiesWindowProxy();
   }
}
