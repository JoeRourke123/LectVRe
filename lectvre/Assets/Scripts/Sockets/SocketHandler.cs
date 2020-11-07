using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class SocketHandler
{
    private ClientWebSocket ws;
    private Uri serverUri;
    private string serverURL = "";
    private UInt64 MAXSIZE = 2048;
    private static int TIMEOUT = 12;
    
    // Start is called before the first frame update
    public SocketHandler(string url)
    {
        ws = new ClientWebSocket();
        serverURL = url;
        serverUri = new Uri(this.serverURL);
    }

    public async Task close()
    {
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Object Destroyed", CancellationToken.None);
        return;
    }

    public async Task Connect()
    {
        await ws.ConnectAsync(serverUri, CancellationToken.None);
        while (ws.State == WebSocketState.Connecting)
        {
            Debug.Log("Waiting to Connect...");
        }
        Debug.Log("Connect status: " + ws.State);
        return;
    }

    public async Task ReConnect()
    {
        for (int i = 0; i < TIMEOUT && ws.State != WebSocketState.Open; i++)
        {
            Task.Delay(50).Wait(); 
            await Connect();
        }        
        return;
    }

    public async Task Send(string message)
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
            await ReConnect();
        }
        catch (InvalidOperationException e)
        {
             e.ToString();
             await ReConnect();
         }
         return;
    }

    public async Task<PlayerPosition> Receive()
    {
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MAXSIZE]);
        PlayerPosition player = new PlayerPosition();
        try
        {
            await ws.ReceiveAsync(buffer, CancellationToken.None);
            Debug.Log(buffer);
            string str = System.Text.Encoding.Default.GetString(buffer.ToArray());
            JsonUtility.FromJsonOverwrite(str, player);
            Debug.Log(player);
        }
        
        catch (ObjectDisposedException e)
        {
            e.ToString();
            await ReConnect();
        }
        catch (InvalidOperationException e) 
        {
            e.ToString();
            await ReConnect();
        }
        return player;
    }
}



