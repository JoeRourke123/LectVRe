using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

[Serializable]
public class RecMessage : Message
{
    public string id;

    public RecMessage(float x, float y, float z, float r, string type, string id) : base(x, y, x, r, type)
    {
        this.id = id;
    }

    public RecMessage() {
        
    }
}
    