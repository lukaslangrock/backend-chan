namespace backend.ProtocolObjects;

public class MemberList(UserDescription[] userDescription)
{
    private readonly UserDescription[] _userDescription = userDescription;
}