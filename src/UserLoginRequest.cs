namespace backend;

public class UserLoginRequest
{
    private string _username;
    private string _password;

    public string Username
    {
        get => _username;
        set => _username = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Password
    {
        get => _password;
        set => _password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public UserLoginRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}