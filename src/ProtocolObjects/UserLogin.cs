namespace backend;

public class UserLogin
{
    private readonly bool _success = false;
    private readonly int _clientId;

    public bool Success
    {
        get => _success;
    }

    public int ClientId
    {
        get => _clientId;
    }

    public UserLogin(bool success, int clientId)
    {
        _success = success;
        _clientId = clientId;
    }
}