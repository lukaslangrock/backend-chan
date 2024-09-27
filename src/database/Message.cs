namespace backend.database;

public class Message(int id, int timestamp, int roomId, int senderId, string text)
{
    public readonly int Id = id;
    public readonly int Timestamp = timestamp;
    public readonly int RoomId = roomId;
    public readonly int SenderId = senderId;
    public readonly string Text = text;
}