using System;

namespace ASBMessageTool.Application.Config;

public static class StaticConfig
{
    public static readonly string ApplicationDisplayName = "Azure service bus message tool";
    public static readonly string AuthorDisplayInfo = "Grzegorz Wolszczak";
    public static readonly string LicenseName = "BSD 3-Clause License";
    public static readonly string ApplicationName = "AzureServiceBusMessageTool";
    public static readonly string ConfigFileName = $"{ApplicationName}.settings.json";
    // todo: below link is invalid, update it when repository is not private anymore
    public static readonly string ThisRepositoryUrl = "https://github.com/grzegorz-wolszczak/asb-message-tool";

    public static readonly TimeSpan NextMessageReceiveDelayTimeSpan = TimeSpan.FromMilliseconds(500);
    public static readonly TimeSpan MessageReceiverReceiveTimeout = TimeSpan.FromSeconds(30);
}
