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
    private string name = "Fuck You";
    private Dictionary<string, UserData> users = new Dictionary<string, UserData>();
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
        await Send(new MainMessage("join", new UserMessage(roomId, name, "join", new MinifigureData(1, 1, 1, 1 ,1, 1))));
        Debug.Log("Roomed Joined");
        InvokeRepeating("SendPosition", 0.02f, INTERVAL);
        return;
    }
    private async void SendPosition()
    {
        Message msg = new Message(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z,
            Camera.main.transform.eulerAngles.y,
            new MinifigureData(0,0,0,0,0,0)
        );
        await Send(new MainMessage("position", msg));
    }

    private async Task Send(IMessageInterface message) {
        if(ws.State == WebSocketState.Open) 
        {
            await ws.SendText(message.toJson());
            Debug.Log("Message Sent: " + message.toJson());
        }
        return;
    }
    private IMessageInterface Receive(byte[] bytes) {
        Debug.Log("Bytes: " + bytes);
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("OnMessage! " + message);

        MainMessage mainMessage = JsonUtility.FromJson<MainMessage>(message);
        switch(mainMessage.type) {
            case "position":
            RecMessage rm = JsonUtility.FromJson<RecMessage>(mainMessage.message);
            mainMessage.finalMessage = rm;
            UpdateUsers(rm);
            break;
            case "join":
            UserMessage um = JsonUtility.FromJson<UserMessage>(mainMessage.message);
            mainMessage.finalMessage = um;
            break;
            case "create_room":
            RoomJoin rj = JsonUtility.FromJson<RoomJoin>(mainMessage.message);
            mainMessage.finalMessage = rj;
            break;
        }
        return mainMessage;
    }
    private void UpdateRoom(UserMessage message) {
        this.userId = message.user;
        this.roomId = message.roomId;
    }
    private void UpdateUsers(RecMessage message) {
        if(message.id != this.userId) {
            if(users.ContainsKey(message.id)) {
                GameObject user = users[message.id].gameObject;
                user.transform.position = message.toVector3();

                Vector3 newRotation = user.transform.eulerAngles;
                newRotation.y = message.getAngle();
                user.transform.eulerAngles = newRotation;
            }
            else {
                users.Add(message.id, new UserData(message.id, message.name, Instantiate(userObject, message.toVector3(), message.toQuaternion(), gameObject.transform)));
            }
        }
    }
    private async void OnApplicationQuit()
    {
        await ws.Close();
    }
    
}



