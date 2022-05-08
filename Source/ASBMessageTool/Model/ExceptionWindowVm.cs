using System;

namespace ASBMessageTool.Model;

public class ExceptionWindowVm
{
    public Exception Exception { get; }

    public string ExceptionType { get; }

    public ExceptionWindowVm(Exception exc)
    {
        Exception = exc;
        ExceptionType = exc.GetType().FullName;
    }
}
