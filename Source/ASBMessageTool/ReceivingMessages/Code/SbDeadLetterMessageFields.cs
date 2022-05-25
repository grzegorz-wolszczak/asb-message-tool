using ASBMessageTool.Model;

namespace ASBMessageTool.ReceivingMessages.Code;

public class SbDeadLetterMessageFields
{
    public SbMessageField<string> DeadLetterReason { get; } = new("DeadLetterReason");
    public SbMessageField<string> DeadLetterErrorDescription { get; } = new("DeadLetterErrorDescription");
}
