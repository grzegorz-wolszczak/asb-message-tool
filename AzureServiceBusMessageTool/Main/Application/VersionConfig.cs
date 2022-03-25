﻿using System;

namespace Main.Application;

public static class VersionConfig
{
    public const string VersionString = "0.15.0";
    public static readonly Version Version = Version.Parse(VersionString);
}
