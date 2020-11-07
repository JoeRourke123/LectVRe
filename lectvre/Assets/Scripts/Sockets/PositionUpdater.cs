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
    private static float INTERVAL = 0.05f;
    
    // Start is called before the first frame update
    async void Start()
    {
        sh = new SocketHandler("");
        await sh.Connect();

        InvokeRepeating("SendPosition", 0.02f, INTERVAL);
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
        Debug.Log(Camera.main.transform.position);
        Message msg = new Message();
        msg.x = Camera.main.transform.position.x;
        msg.y = Camera.main.transform.position.y;
        msg.z = Camera.main.transform.position.z;
        msg.type = "position";
        msg.id = "";
        await sh.Send(JsonUtility.ToJson(msg));
        await sh.Receive();
    }
}



