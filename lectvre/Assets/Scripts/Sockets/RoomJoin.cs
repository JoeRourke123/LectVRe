using System;

/**
* Used for creating a new room
**/
[Serializable]
public class RoomJoin : MainMessage
{
    public string room;
    public string user;

    public RoomJoin(string type, string room, string user) : base(type){
        this.room = room;
        this.user = user;
    }

    new public string toJson() {
        return $"{{\"type\":\"{type}\", \"room\":\"{room}\", \"user\":\"{user}\"}}";
    }
}