using ASBMessageTool.Model;

namespace ASBMessageTool.ReceivingMessages;

public class SbDeadLetterMessageFields
{
    public SbMessageField<string> DeadLetterReason { get; } = new("DeadLetterReason");
    public SbMessageField<string> DeadLetterErrorDescription { get; } = new("DeadLetterErrorDescription");
}
