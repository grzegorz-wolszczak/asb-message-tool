using System;

namespace Main.ExceptionHandling;

public class AsbMessageToolException : ApplicationException
{
    public AsbMessageToolException(string message) : base(message)
    {
    }
}
