namespace backend;

public class UserDescriptionRequest
{
    private readonly int _userId;

    public int UserId
    {
        get => _userId;
    }

    public UserDescriptionRequest(int userId)
    {
        _userId = userId;
    }
}