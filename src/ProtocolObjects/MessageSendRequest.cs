namespace backend.ProtocolObjects;

public class MessageSendRequest(int clientId, int roomId, string text)
{
    private readonly int _clientId = clientId;
    private readonly int _roomId = roomId;
    private readonly string _text = text;
}