namespace backend;

public class MemberList
{
    private readonly UserDescription[] _userDescription;

    public UserDescription[] UserDescription
    {
        get => _userDescription;
    }

    public MemberList(UserDescription[] userDescription)
    {
        _userDescription = userDescription;
    }
}