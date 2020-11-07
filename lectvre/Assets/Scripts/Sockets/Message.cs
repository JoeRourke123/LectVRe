public class Message
{
    private float x;
    private float y;    
    private float z;
    private float r;    
    private string id;
    private string type;

    public Message(float x, float y, float z, float r, string id, string type) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.r = r;
        this.id = id;
        this.type = type;
    }

    public Message() {
        
    }
}
    