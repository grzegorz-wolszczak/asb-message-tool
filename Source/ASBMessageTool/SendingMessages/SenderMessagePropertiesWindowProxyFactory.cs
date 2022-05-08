

namespace ASBMessageTool.SendingMessages;

public class SenderMessagePropertiesWindowProxyFactory
{
   public ISenderMessagePropertiesWindowProxy Create()
   {
      return new SenderMessagePropertiesWindowProxy();
   }
}
