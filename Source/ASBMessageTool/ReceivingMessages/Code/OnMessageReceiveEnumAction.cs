namespace ASBMessageTool.ReceivingMessages.Code;

public enum OnMessageReceiveEnumAction
{
    Complete = 0,
    Abandon = 1,
    MoveToDeadLetter = 2
}
