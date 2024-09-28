namespace backend.ProtocolObjects;

public class Message(int userId, int roomId, string name)
{
    private readonly int _userId = userId;
    private readonly int _roomId = roomId;
    private readonly string _name = name;
}