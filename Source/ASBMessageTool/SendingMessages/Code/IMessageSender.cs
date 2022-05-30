using System;
using System.Threading.Tasks;
using Core.Maybe;

namespace ASBMessageTool.SendingMessages.Code;

public interface IMessageSender
{
    Task Send(SenderCallbacks callbacks, ServiceBusMessageSendData messageToSend);
    void Stop();
}

public class MessageSendErrorInfo
{
    public string Message { get; init; }
    public Maybe<Exception> Exception { get; init; }
}
