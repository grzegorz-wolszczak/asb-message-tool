using System;

namespace ASBMessageTool.Application.Config;

public static class VersionConfig
{
    public const string VersionString  = "0.21.1";
    public static readonly Version Version = Version.Parse(VersionString);
}
