namespace backend;

public class UserLoginRequest
{
    private readonly string _username;
    private readonly string _password;

    public string Username
    {
        get => _username;
    }

    public string Password
    {
        get => _password;
    }

    public UserLoginRequest(string username, string password)
    {
        _username = username;
        _password = password;
    }
}