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

    public MinifigureData(int[] data) {
        this.hairColour = data[0];
        this.skinColour = data[1];
        this.torso = data[2];
        this.legs = data[3];
        this.hair = data[4];
        this.head = data[5];
    }

    public MinifigureData() {

    }

    public string toJson() {
        return $"{{\"hair\":{hair}, \"harColour\":{hairColour}, \"skinColour\":{skinColour},\"head\":{head}, \"torso\":{torso}, \"legs\":{legs}}}";
    }

    public int[] toArray() {
        return new int[] {this.hairColour, this.skinColour, this.torso, this.legs, this.hair, this.head};
    }
}
    