using System;

namespace ASBMessageTool.Application;

public static class TimeUtils
{
    public static string GetShortTimestamp()
    {
        var time = DateTime.Now.ToString("HH:mm:ss.fff");
        return $"[{time}]";
    }
}
