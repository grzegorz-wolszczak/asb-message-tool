using System;

namespace Main.Application;

internal class ServiceBusHelperException : Exception
{
   public ServiceBusHelperException(string msg):base(msg)
   {
   }
}
