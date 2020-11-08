using System;
using UnityEngine;
/**
* Class for containing data for repeatably sending the player position
**/
[Serializable]
public class Message : MainMessage
{
    public float x;
    public float y;
    public float z;
    public float r;
    public MinifigureData minifigureData;

    public Message(string type, float x, float y, float z, float r, MinifigureData minifigureData) : base(type) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.r = r;
        this.minifigureData = minifigureData;
    }

    public Message() {

    }

    new public string toJson() {
        return $"{{\"type\":\"{type}\", \"x\":{x}, \"y\":{y}, \"z\":{z}, \"r\":{r}, \"minifig\":{minifigureData.toJson()}}}";
    }

    public Vector3 toVector3() {
        return new Vector3(x, y, z);
    }

    public float getAngle() {
        return r;
    }
    
    public Vector3 toEuelerAngle() {
        return new Vector3(0, r, 0);
    }

    public Quaternion toQuaternion() {
        return Quaternion.Euler(0, r, 0);
    }
}
    