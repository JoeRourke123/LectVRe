using System.Threading.Tasks;
using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

public class SocketHandler : MonoBehaviour
{
    private WebSocket ws;
    private string serverURL = "ws://192.168.0.24:8000/ws/";
    private static float INTERVAL = 0.05f;
    private string userId;
    private string roomId = "db1f5";
    private string username = "Fuck You";
    public GameObject userObject;
    
    // Start is called before the first frame update
    async void Start()
    {
        ws = new WebSocket(serverURL);

        ws.OnOpen += () => 
        {
            Debug.Log("Connected");
        };

        ws.OnError += (e) => 
        {
            Debug.Log("Error: " + e);
        };

        ws.OnClose += (e) => 
        {
            Debug.Log("Connection Closed");
        };

        ws.OnMessage += (bytes) =>
        {
            Receive(bytes);
        };
        ws.Connect();
        while(ws.State != WebSocketState.Open);
        await JoinRoom();
    }
    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
            ws.DispatchMessageQueue();
        #endif
    }
    private async Task JoinRoom() {
        Debug.Log("Joining Room...");
        await Send(new UserMessage("join", roomId, username, "join", new MinifigureData(1, 1, 1, 1 ,1, 1)));
        Debug.Log("Roomed Joined");
        InvokeRepeating("SendPosition", 0.5f, INTERVAL);
        return;
    }
    private async void SendPosition()
    {
        Message msg = new Message(
            "position",
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z,
            Camera.main.transform.eulerAngles.y,
            new MinifigureData(0,0,0,0,0,0)
        );
        Debug.Log(msg.toJson());
        await Send(msg);
    }

    private async Task Send(IMessageInterface message) {
        if(ws.State == WebSocketState.Open) 
        {
            if(message is Message) {
                await ws.SendText(((Message)message).toJson());
                Debug.Log("Message Sent: " + ((Message)message).toJson());
            }
            else if(message is RecMessage) {
                await ws.SendText(((RecMessage)message).toJson());
                Debug.Log("Message Sent: " + ((RecMessage)message).toJson());
            }
            else if(message is RoomJoin) {
                await ws.SendText(((RoomJoin)message).toJson());
                Debug.Log("Message Sent: " + ((RoomJoin)message).toJson());
            }
            else if(message is UserMessage) {
                await ws.SendText(((UserMessage)message).toJson());
                Debug.Log("Message Sent: " + ((UserMessage)message).toJson());
            }
            else {
                await ws.SendText(((IMessageInterface)message).toJson());
                Debug.Log("Message Sent: " + ((IMessageInterface)message).toJson());
            }
        }
        return;
    }
    private IMessageInterface Receive(byte[] bytes) {
        // Debug.Log("Bytes: " + bytes);
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("OnMessage: " + message);

        MainMessage mainMessage = JsonUtility.FromJson<MainMessage>(message);
        switch(mainMessage.type) {
            case "position":
            RecMessage rm = JsonUtility.FromJson<RecMessage>(message);
            UpdateUsers(rm);
            return rm;
            case "join":
            UserMessage um = JsonUtility.FromJson<UserMessage>(message);
            UpdateRoom(um);
            return um;
            case "create_room":
            default:
            RoomJoin rj = JsonUtility.FromJson<RoomJoin>(message);            
            return rj;
        }
    }
    private void UpdateRoom(UserMessage message) {
        Debug.Log("NEW MESSAGE: " + message);
        this.userId = message.user;
        this.roomId = message.roomId;
    }
    private void UpdateUsers(RecMessage message) {
        UserData[] childrenData = gameObject.GetComponentsInChildren<UserData>();
        if(message.user != this.userId) {
            UserData child = null;
            foreach(UserData userData in childrenData) {
                if(userData.id == message.user) {
                    child = userData;
                    break;
                }
            }
            if(child != null) {
                child.gameObject.name = message.username;
                child.gameObject.transform.position = message.toVector3();
                Vector3 newRotation = child.gameObject.transform.eulerAngles;
                newRotation.y = message.getAngle();
                child.gameObject.transform.eulerAngles = newRotation;
            }
            else {
                GameObject newChild = Instantiate(userObject, message.toVector3(), message.toQuaternion(), gameObject.transform);
                UserData data = newChild.GetComponent<UserData>();
                
                data.username = message.username;
                data.id = message.user;
            }
        }
    }
    private async void OnApplicationQuit()
    {
        await ws.Close();
    }
    
}



