using ASBMessageTool.Application;

namespace ASBMessageTool;

public class InvalidConfigurationException : AsbMessageToolException
{
    public InvalidConfigurationException(string message) : base(message)
    {
    }
}
