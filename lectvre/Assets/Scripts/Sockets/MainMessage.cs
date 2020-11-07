using System;

[Serializable]
public class MainMessage : IMessageInterface
{
    public string type;
    public string message;
    public IMessageInterface finalMessage;

    public MainMessage(string type, string message) {
        this.type = type;
        this.message = message;
    }

    public MainMessage(string type, IMessageInterface finalMessage) {
        this.type = type;
        this.finalMessage = finalMessage; 
    }

    public MainMessage() {

    }

    public string toJson() {
        return $"{{\"type\":\"{type}\", \"message\":{finalMessage.toJson()}}}";
    }
}
    