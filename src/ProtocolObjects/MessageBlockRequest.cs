namespace backend.ProtocolObjects;

public class MessageBlockRequest (int roomId, long startTs, long endTs)
{
    private readonly int _roomId = roomId;
    private readonly long _startTs = startTs;
    private readonly long _endTs = endTs; 
}