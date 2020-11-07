﻿using System;
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
    private static float INTERVAL = 0.1f;
    
    // Start is called before the first frame update
    async void Start()
    {
        sh = new SocketHandler("ws://192.168.0.24:8000/ws/");
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
        Message msg = new Message(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z,
            Camera.main.transform.eulerAngles.y,
            "position",
            ""
        );
        Debug.Log("JSON: " + JsonUtility.ToJson(msg));
        await sh.Send(JsonUtility.ToJson(msg));
        await sh.Receive();
    }
}


