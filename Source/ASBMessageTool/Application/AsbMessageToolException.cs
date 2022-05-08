using System;

namespace ASBMessageTool.Application;

public class AsbMessageToolException : Exception
{
    public AsbMessageToolException(string msg):base(msg)
    {
    }
}
