namespace Main.ExceptionHandling;

public class InvalidConfigurationException : AsbMessageToolException
{
    public InvalidConfigurationException(string message) : base(message)
    {
    }
}
