using System;

/**
* Class for containing data for the look of a minifigure
**/
[Serializable]
public class MinifigureData : IMessageInterface
{
    public int hair; 
    public int hairColour;
    public int skinColour;
    public int head;
    public int torso;
    public int legs;

    public MinifigureData(int hair, int hairColour, int skinColour, int head, int torso, int legs) {
        this.hair = hair;
        this.hairColour = hairColour;
        this.skinColour = skinColour;
        this.head = head;
        this.torso = torso;
        this.legs = legs;
    }

    public MinifigureData() {

    }

    public string toJson() {
        return $"{{\"hair\":{hair}, \"harColour\":{hairColour}, \"skinColour\":{skinColour},\"head\":{head}, \"torso\":{torso}, \"legs\":{legs}}}";
    }
}
    