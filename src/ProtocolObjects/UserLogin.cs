namespace backend.ProtocolObjects;

public class UserLogin
{
    private readonly bool _success = false;
    private readonly int _clientId;

    public UserLogin(bool success, int clientId)
    {
        _success = success;
        _clientId = clientId;
    }
}