using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

[Serializable]
public class Message
{
    public float x;
    public float y;
    public float z;
    public float r;
    public string type;

    public Message(float x, float y, float z, float r, string type) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.r = r;
        this.type = type;
    }

    public Message() {

    }

    public string toJson() {
        return $"{{\"x\":{x}, \"y\":{y}, \"z\":{z}, \"r\":{r}, \"type\":\"{type}\"}}";
    }

    public Vector3 toVector3() {
        return new Vector3(x, y, z);
    }

    public float getAngle() {
        return r;
    }
}
    