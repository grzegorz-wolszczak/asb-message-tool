using System;

namespace Main.Application;

public static class StaticConfig
{
   public static readonly string ApplicationDisplayName = "Azure service bus message tool";
   public static readonly string AuthorDisplayInfo = "Grzegorz Wolszczak";
   public static readonly string LicenseName = "BSD 3-Clause License";
   public static readonly string ApplicationName = "AzureServiceBusMessageTool";
   public static readonly string ConfigFileName = $"{ApplicationName}.settings.json";
   public static readonly string ThisRepositoryUrl = "https://github.com/grzegorz-wolszczak/asb-message-tool";

   public static readonly TimeSpan NextMessageReceiveDelayTimeSpan = TimeSpan.FromMilliseconds(500);
   public static readonly TimeSpan MessageReceiverReceiveTimeout = TimeSpan.FromSeconds(30);
   public const string VersionString = "0.7.0";
   public static readonly Version Version = Version.Parse(VersionString);
}
