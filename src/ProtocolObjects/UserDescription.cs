namespace backend;

public class UserDescription
{
    private readonly int _id;
    private readonly string _displayName;
    private readonly string _onlineStatus;

    public int Id
    {
        get => _id;
    }

    public string DisplayName
    {
        get => _displayName;
    }

    public string OnlineStatus
    {
        get => _onlineStatus;
    }

    public UserDescription(int id, string displayName, string onlineStatus)
    {
        _id = id;
        _displayName = displayName;
        _onlineStatus = onlineStatus;
    }
}