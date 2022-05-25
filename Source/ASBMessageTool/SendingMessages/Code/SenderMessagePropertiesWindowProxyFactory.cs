

namespace ASBMessageTool.SendingMessages.Code;

public class SenderMessagePropertiesWindowProxyFactory
{
   public ISenderMessagePropertiesWindowProxy Create()
   {
      return new SenderMessagePropertiesWindowProxy();
   }
}
