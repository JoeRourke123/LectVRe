using System;

/**
* Class for containing data for joining a room
**/
[Serializable]
public class UserMessage : MainMessage
{
    public string roomId;
    public string name;
    public string user;
    public MinifigureData minifigureData;

    public UserMessage(string type, string roomId, string name, string user, MinifigureData minifigureData) : base(type) {
        this.roomId = roomId;
        this.name = name;
        this.user = user;
        this.minifigureData = minifigureData;
    }
    public UserMessage(string type, string roomId, string name, string user, int head, int torso, int legs, int hair, int hairColour, int skinColour) : base(type){
        this.roomId = roomId;
        this.name = name;
        this.user = user;
        this.minifigureData = new MinifigureData(hair, hairColour, skinColour, head, torso, legs);
    }

    public UserMessage() {

    }

    new public string toJson() {
        return $"{{\"type\":\"{type}\", \"minifig\":{minifigureData.toJson()}, \"name\":\"{name}\", \"room\":\"{roomId}\"}}";
    }
}
    