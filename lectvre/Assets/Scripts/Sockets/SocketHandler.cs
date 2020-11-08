using System;
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
            Camera.main.gameObject.transform.parent.position.x,
            Camera.main.gameObject.transform.parent.position.y,
            Camera.main.gameObject.transform.parent.position.z,
            Camera.main.gameObject.transform.parent.eulerAngles.y,
            new MinifigureData(0,0,0,0,0,0)
        );
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
            if (!String.IsNullOrEmpty(um.slideURL))
            {
	            FetchSlide(um.slideURL, um.slide);
            }
            UpdateRoom(um);
            return um;
            case "slide":
	        SlideChange cs = JsonUtility.FromJson<SlideChange>(message);
	        FetchSlide(cs.slideURL, cs.slide);
	        return cs;
            default:
            case "leave":
            UserData ud = JsonUtility.FromJson<UserData>(message);
            RemoveUser(ud);
            return ud;
        }
    }
    private void UpdateRoom(UserMessage message) {
        this.userId = message.user;
        this.roomId = message.roomId;
        Camera.main.gameObject.transform.parent.position = GameObject.Find("Seats").gameObject.transform.Find(message.seat.ToString()).position;
    }

    private void FetchSlide(string slideURL, int slide)
    {
	    StartCoroutine(GameObject.Find("BoardController").GetComponent<LoadImage>().getImg(slide, slideURL));
    }
    private void UpdateUsers(RecMessage message) {
        UserData[] childrenData = gameObject.GetComponentsInChildren<UserData>();
        if(message.user != this.userId) {
            UserData child = null;
            foreach(UserData userData in childrenData) {
                if(userData.user == message.user) {
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
                data.user = message.user;
            }
        }
    }

    private void RemoveUser(UserData userData) {
        UserData[] childrenData = gameObject.GetComponentsInChildren<UserData>();
        foreach(UserData ud in childrenData) {
            if(ud.user == userData.user) {
                Destroy(ud.gameObject);
                return;
            }
        }
    }
    private async void OnApplicationQuit()
    {
        await ws.Close();
    }

}



