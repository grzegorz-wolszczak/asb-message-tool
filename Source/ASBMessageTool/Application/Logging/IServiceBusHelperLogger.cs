using System;

namespace ASBMessageTool.Application.Logging;

public interface IServiceBusHelperLogger
{
    public void LogError(string msg);
    void LogInfo(string msg);
    void LogException(Exception exception);
    void LogException(string msg, Exception exception);
    void LogWarning(string s);
}
