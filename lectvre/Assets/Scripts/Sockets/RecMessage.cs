using System;

/**
* Class for containing data for recieving player positions.
**/
[Serializable]
public class RecMessage : Message
{
    public string user;
    public string username;

    public RecMessage(string type, float x, float y, float z, float r, string username, MinifigureData minifigureData, string user) : base(type, x , y, z, r, minifigureData)
    {
        this.user = user;
        this.username = username;
    }

    public RecMessage() {
        
    }
    new public string toJson() {
        return $"{{\"type\":\"{type}\", \"user\":\"{user}\", \"name\":\"{username}\", \"x\":{x}, \"y\":{y}, \"z\":{z}, \"r\":{r}, \"minifig\":{minifigureData.toJson()}}}";
    }
}
    