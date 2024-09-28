namespace backend.ProtocolObjects;

public class RoomDescription(string displayName, int id, MemberList[] memberList)
{
    private readonly string _displayName = displayName;
    private readonly int _id = id;
    private readonly MemberList[] _memberList = memberList;
}