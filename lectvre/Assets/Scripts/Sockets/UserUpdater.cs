using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

public class UserUpdater : MonoBehaviour
{

    private SocketHandler sh;
    private static float INTERVAL = 0.05f;

    // Start is called before the first frame update
    async void Start()
    {
        sh = new SocketHandler("");
        await sh.Connect();

        InvokeRepeating("ReceiveLoop", 0.02f, INTERVAL);
    }

    void FixedUpdate()
    {
        
    }

    async void OnDestroy()
    {
        await sh.close();
    }


    async void ReceiveLoop()
    {
        while (true)
        {
            PlayerPosition player = await sh.Receive();
        }
    }
    
}