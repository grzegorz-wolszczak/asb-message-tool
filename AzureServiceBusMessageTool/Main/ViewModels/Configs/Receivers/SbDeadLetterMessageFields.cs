using Main.ViewModels.Configs.Senders.MessagePropertyWindow;

namespace Main.ViewModels.Configs.Receivers;

public class SbDeadLetterMessageFields
{
    public SbMessageField<string> DeadLetterReason { get; } = new("DeadLetterReason");
    public SbMessageField<string> DeadLetterErrorDescription { get; } = new("DeadLetterErrorDescription");
}
