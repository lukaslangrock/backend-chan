namespace backend;

public class UserDescription
{
    private int _id;
    private string _displayName;
    private string _onlineStatus;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string DisplayName
    {
        get => _displayName;
        set => _displayName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string OnlineStatus
    {
        get => _onlineStatus;
        set => _onlineStatus = value ?? throw new ArgumentNullException(nameof(value));
    }

    public UserDescription(int id, string displayName, string onlineStatus)
    {
        _id = id;
        _displayName = displayName;
        _onlineStatus = onlineStatus;
    }
}