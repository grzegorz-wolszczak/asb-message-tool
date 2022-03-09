﻿using System;

namespace Main.Application;

public static class AppConstants
{
  public static readonly Version Version = new Version(0, 5, 1);
  public static readonly string ApplicationDisplayName = "Azure service bus message tool";
  public static readonly string AuthorDisplayInfo = "Grzegorz Wolszczak";
  public static readonly string LicenseName = "BSD 3-Clause License";
  public static readonly string ApplicationName = "AzureServiceBusMessageTool";
  public static readonly string ConfigFileName = $"{ApplicationName}.settings.json";
  public static readonly string ThisRepositoryUrl = "https://github.com/grzegorz-wolszczak/asb-message-tool";

  public static readonly TimeSpan NextMessageReceiveDelayTimeSpan = TimeSpan.FromMilliseconds(500);
  public static readonly TimeSpan MessageReceiverReceiveTimeout = TimeSpan.FromSeconds(30);
}