using System;

/**
* Class for containing data for recieving player positions.
**/
[Serializable]
public class RecMessage : Message
{
    public string id;
    public string name;

    public RecMessage(float x, float y, float z, float r, string name, MinifigureData minifigureData, string id) : base(x, y, x, r, minifigureData)
    {
        this.id = id;
        this.name = name;
    }

    public RecMessage() {
        
    }
    new public string toJson() {
        return $"{{\"id\":\"{id}\", \"name\":\"{name}\", \"x\":{x}, \"y\":{y}, \"z\":{z}, \"r\":{r}, \"minifig\":{minifigureData.toJson()}}}";
    }
}
    