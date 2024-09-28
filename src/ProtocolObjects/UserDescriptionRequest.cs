namespace backend;

public class UserDescriptionRequest
{
    private int _userId;

    public int UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public UserDescriptionRequest(int userId)
    {
        _userId = userId;
    }
}