using System;
using System.Collections.Generic;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekerCallbacks
{
    public Action<Exception> OnPeekerFailure { get; init; }
    public Action<List<PeekedMessage>> OnAllMessagesPeeked { get; init; }
    public Action OnPeekerFinished { get; init; }
    public Action OnPeekerStarted { get; init; }

    public Action OnPeekerInitializing { get; init; }
}
