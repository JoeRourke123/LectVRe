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
    private ClientWebSocket ws;
    private Uri serverUri;
    private string serverURL = "";
    private UInt64 MAXSIZE = 2048;
    private static int TIMEOUT = 12;
    private static float INTERVAL = 0.05f;
    
    // Start is called before the first frame update
    async void Start()
    {
        ws = new ClientWebSocket();
        serverUri = new Uri(this.serverURL);
        await Connect();

        InvokeRepeating("SendPosition", 0.02f, INTERVAL);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    async void OnDestroy()
    {
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Object Destroyed", CancellationToken.None);
    }

    void SendPosition()
    {
        Debug.Log(Camera.main.transform.position);
        Send($"{{x:{Camera.main.transform.position.x},y:{Camera.main.transform.position.y},z:{Camera.main.transform.position.z},type:'position'}}");
    }

    async Task Connect()
    {
        await ws.ConnectAsync(serverUri, CancellationToken.None);
        while (ws.State == WebSocketState.Connecting)
        {
            Debug.Log("Waiting to Connect...");
        }
        Debug.Log("Connect status: " + ws.State);
        return;
    }

    async void ReConnect()
    {
        for (int i = 0; i < TIMEOUT && ws.State != WebSocketState.Open; i++)
        {
            Task.Delay(50).Wait(); 
            await Connect();
        }        
        
    }

    async void Send(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        ArraySegment<Byte> msg = new ArraySegment<Byte>(buffer);
        try
        {
            await ws.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
            Debug.Log(buffer);
        }
        
        catch (ObjectDisposedException e)
        {
            e.ToString();
            ReConnect();
        }
        catch (InvalidOperationException e)
        {
             e.ToString();
             ReConnect();
         }
    }

    async void Recieve()
    {
        ArraySegment<Byte> buffer = new ArraySegment<Byte>(new byte[MAXSIZE]);
        try
        {
            await ws.ReceiveAsync(buffer, CancellationToken.None);
            Debug.Log(buffer);
        }
        
        catch (ObjectDisposedException e)
        {
            e.ToString();
            ReConnect();
        }
        catch (InvalidOperationException e) 
        {
            e.ToString();
            ReConnect();
        }
    }
}