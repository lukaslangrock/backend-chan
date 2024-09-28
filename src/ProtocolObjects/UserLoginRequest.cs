namespace backend.ProtocolObjects;

public class UserLoginRequest(string username, string password)
{
    private readonly string _username = username;
    private readonly string _password = password;
}