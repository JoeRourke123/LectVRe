using System;
using UnityEngine;

[Serializable]
public class MainMessage : IMessageInterface
{
    public string type;

    public MainMessage(string type) {
        this.type = type;
    }

    public MainMessage() {

    }

    public string toJson() {
        return $"{{\"type\":\"{type}\"}}";
    }
}
    