using System;
using System.Text;

namespace ASBMessageTool.Application.Logging;

public interface ILogContentAppender
{
    public void AddContent(string msg);
}

public class ServiceBusHelperLogger : IServiceBusHelperLogger
{
    private readonly ILogContentAppender _contentAppender;

    public ServiceBusHelperLogger(ILogContentAppender contentAppender)
    {
        _contentAppender = contentAppender;
    }

    private static string FlattenException(Exception exception)
    {
        var stringBuilder = new StringBuilder();

        while (exception != null)
        {
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);

            exception = exception.InnerException;
        }

        return stringBuilder.ToString();
    }

    public void LogError(string msg)
    {
        var errorMsg = WrapWithTimestamp($"Error: {msg}\n");
        _contentAppender.AddContent(errorMsg);
    }

    public void LogInfo(string msg)
    {
        var errorMsg = WrapWithTimestamp($"Info : {msg}\n");
        _contentAppender.AddContent(errorMsg);
    }

    public void LogException(Exception exception)
    {
        var flattenedException = FlattenException(exception);
        LogError(flattenedException);
    }

    public void LogException(string msg, Exception exception)
    {
        var flattenedException = FlattenException(exception);

        LogError($"{msg} {flattenedException}");
    }

    public void LogWarning(string msg)
    {
        var message = WrapWithTimestamp($"Warn : {msg}\n");
        _contentAppender.AddContent(message);
    }

    private string WrapWithTimestamp(string msg)
    {
        return $"{TimeUtils.GetShortTimestamp()} {msg}";
    }
}
