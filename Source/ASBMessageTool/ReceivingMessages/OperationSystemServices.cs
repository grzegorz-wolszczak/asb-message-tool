using System.Windows;

public interface IOperationSystemServices
{
    void SetClipboardText(string text);
}

public class OperationSystemServices : IOperationSystemServices
{
    public void SetClipboardText(string text)
    {
        Clipboard.SetText(text);
    }
}
