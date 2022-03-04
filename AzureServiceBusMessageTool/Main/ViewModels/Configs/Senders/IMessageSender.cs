using System;
using Core.Maybe;

namespace Main.ViewModels.Configs.Senders;

public interface IMessageSender
{
   Maybe<MessageSendErrorInfo> Send(MessageToSendData msgToSend);
}

public class MessageSendErrorInfo
{
   public string Message { get; init; }
   public Maybe<Exception> Exception { get; init; }
}
