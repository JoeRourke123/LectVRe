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
        Debug.Log("Connect status: " + ws.State);
        if(ws.State != WebSocketState.Open) await ReConnect();
        return;
    }

    public async Task ReConnect()
    {
        await ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Need to close connection before reconnection", CancellationToken.None);
        for (int i = 0; i < TIMEOUT && ws.State != WebSocketState.Open; i++)
        {
            Task.Delay(100).Wait(); 
            await ws.ConnectAsync(serverUri, CancellationToken.None);
        }        
        return;
    }

    public async Task Send(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        ArraySegment<Byte> msg = new ArraySegment<Byte>(buffer);
        Debug.Log("Message: " + msg);
        try
        {
            await ws.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
            Debug.Log("Buffer: " + buffer);
        }
        catch(WebSocketException e) {
            Debug.Log(e);
            await ReConnect();
        }
        catch (ObjectDisposedException e)
        {
            Debug.Log(e);
            await ReConnect();
        }
        catch (InvalidOperationException e)
        {
            Debug.Log(e);
            await ReConnect();
         }
         return;
    }

    public async Task<Message> Receive()
    {
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MAXSIZE]);
        Message message = new Message();
        try
        {
            Debug.Log("Recieving Data");
            await ws.ReceiveAsync(buffer, CancellationToken.None);
            string str = System.Text.Encoding.Default.GetString(buffer.ToArray());
            JsonUtility.FromJsonOverwrite(str, message);
        }
        catch(WebSocketException e) 
        {
            Debug.Log(e);
            await ReConnect();
        }
        catch (ObjectDisposedException e)
        {
            Debug.Log(e);
            await ReConnect();
        }
        catch (InvalidOperationException e) 
        {
            Debug.Log(e);
            await ReConnect();
        }
        return message;
    }
}



