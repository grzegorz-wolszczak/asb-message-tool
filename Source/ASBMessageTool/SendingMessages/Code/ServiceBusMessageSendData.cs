using System.Collections.Generic;
using ASBMessageTool.Model;

namespace ASBMessageTool.SendingMessages.Code;

public record ServiceBusMessageSendData
{
    public string ConnectionString { get; init; }
    public string QueueOrTopicName { get; init; }
    public string MsgBody { get; init; }
    public SbMessageStandardFields Fields { get; init; }
    public IList<SBMessageApplicationProperty> ApplicationProperties { get; init; }
    public string ConfigName { get; init; }
}
