using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

public class PositionUpdater : MonoBehaviour
{
    private SocketHandler sh;
    private static float INTERVAL = 0.5f;
    
    // Start is called before the first frame update
    async void Start()
    {
        sh = new SocketHandler("ws://192.168.0.24:8000/ws/");
        await sh.Connect();
        await sh.Send("{\"type\": \"create_group\", \"name\": \"Joe Rourke\"}");

        InvokeRepeating("SendPosition", 0.02f, INTERVAL);
        ReceiveLoop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    async void OnDestroy()
    {
        await sh.close();
    }

    async void SendPosition()
    {
        Message msg = new Message(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z,
            Camera.main.transform.eulerAngles.y,
            "position"
        );

        await sh.Send(msg.toJson());
        Message message = await sh.Receive();
    }

    async void ReceiveLoop()
    {
        while(true) {
            RecMessage message = await sh.Receive();
            UpdateUsers(message);
        }
    }

    void UpdateUsers(RecMessage message) {
        GameObject.Find("UserManager").SendMessage("UpdateUsers", message);
    }
}



