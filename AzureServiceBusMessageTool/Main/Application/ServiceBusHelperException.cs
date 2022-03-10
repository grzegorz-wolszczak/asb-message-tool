using System;

namespace Main.Application;

public class ServiceBusHelperException : Exception
{
    public ServiceBusHelperException(string msg):base(msg)
    {
    }
}