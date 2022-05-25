using System.Collections.Generic;
using ASBMessageTool.PeekingMessages.Code;
using ASBMessageTool.ReceivingMessages.Code;
using ASBMessageTool.SendingMessages.Code;
using nucs.JsonSettings;

namespace ASBMessageTool.Application.Persistence;

public sealed class AsbToolPersistentSettings : JsonSettings
{
    public override string FileName { get; set; } = string.Empty;

    public MainWindowSettings MainWindowSettings { get; set; }
    public List<SenderConfigModel> SendersConfig { get; set; }
    public List<ReceiverConfigModel> ReceiversConfig { get; set; }
    
    public List<PeekerConfigModel> PeekersConfig { get; set; }
}
