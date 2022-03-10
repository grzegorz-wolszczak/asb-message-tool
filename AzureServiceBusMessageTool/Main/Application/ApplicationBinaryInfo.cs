using System.IO;

namespace Main.Application;

public class ApplicationBinaryInfo
{
    public string ApplicationName { get; }
    public string BinaryFilePath { get; }
    public string ApplicationDirectory { get; }

    public ApplicationBinaryInfo(
        string applicationName,
        string binaryFilePath)
    {
        ApplicationName = applicationName;
        BinaryFilePath = binaryFilePath;
        ApplicationDirectory = Directory.GetParent(BinaryFilePath).ToString();
    }
}