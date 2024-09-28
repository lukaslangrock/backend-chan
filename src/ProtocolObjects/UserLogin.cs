namespace backend;

public class UserLogin
{
    private bool _success = false;
    private int _clientId;

    public bool Success
    {
        get => _success;
        set => _success = value;
    }

    public int ClientId
    {
        get => _clientId;
        set => _clientId = value;
    }

    public UserLogin(bool success, int clientId)
    {
        Success = success;
        ClientId = clientId;
    }
}