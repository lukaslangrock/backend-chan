namespace backend.database;

public class User(int id, string username, string password, string displayName, OnlineStatus onlineStatus)
{
    public readonly int Id = id;
    public readonly string Username = username;
    public readonly string Password = password;
    public readonly string DisplayName = displayName;
    public readonly OnlineStatus OnlineStatus = onlineStatus;
}
