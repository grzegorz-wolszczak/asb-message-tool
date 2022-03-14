using System;
using System.Threading.Tasks;
using Core.Maybe;

namespace Main.ViewModels.Configs.Senders;

public interface IMessageSender
{
    Task<Maybe<MessageSendErrorInfo>> Send(ServiceBusMessageSendData senderConfig);
}

public class MessageSendErrorInfo
{
    public string Message { get; init; }
    public Maybe<Exception> Exception { get; init; }
}
