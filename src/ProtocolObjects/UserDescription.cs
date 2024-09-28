namespace backend;

public class UserDescription(int id, string displayName, string onlineStatus)
{
    private readonly int _id = id;
    private readonly string _displayName = displayName;
    private readonly string _onlineStatus = onlineStatus;
}