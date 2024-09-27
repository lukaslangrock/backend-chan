using backend.ProtocolObjects;

namespace backend;

public class UserDescription(int id, string displayName, UserOnlineStatus onlineStatus)
{
    private readonly int _id = id;
    private readonly string _displayName = displayName;
    private readonly UserOnlineStatus _onlineStatus = onlineStatus;
}