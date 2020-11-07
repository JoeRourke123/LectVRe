using System;

/**
* Used for creating a new room
**/
[Serializable]
public class RoomJoin : IMessageInterface
{
    public string room;
    public string user;

    public string toJson() {
        return $"{{\"room\":\"{room}\", \"user\":\"{user}\"}}";
    }
}