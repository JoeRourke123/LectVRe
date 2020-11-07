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
    private int TIMEOUT = 12;
    private static int TOTAL_TIMEOUT = 12;
    
    // Start is called before the first frame update
    public SocketHandler(string url)
    {
        ws = new ClientWebSocket();
        serverURL = url;
        serverUri = new Uri(this.serverURL);
    }

    public async Task close()
    {
        Debug.Log("Closing...");
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Object Destroyed", CancellationToken.None);
        Debug.Log("Connect status: " + ws.State);
        return;
    }

    public async Task Connect()
    {
        Debug.Log("Connecting...");
        if(ws.State == WebSocketState.Aborted) {
            ws = new ClientWebSocket();
        }
        for (; TIMEOUT > 0 && ws.State != WebSocketState.Open; TIMEOUT--)
        {
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Task.Delay(100).Wait(); 
        }
        Debug.Log("Connect status: " + ws.State);
        if(ws.State != WebSocketState.Open) {
            throw new TimeoutException();
        }
        TIMEOUT = TOTAL_TIMEOUT;
        return;
    }

    public async Task Send(string message)
    {
        checkStatus();
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        ArraySegment<Byte> msg = new ArraySegment<Byte>(buffer);
        try
        {
            Debug.Log("Sending: " + msg);
            await ws.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        catch(Exception e) 
        {
            Debug.Log(e);
        }
        return;
    }

    public async Task<Message> Receive()
    {
        checkStatus();
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MAXSIZE]);
        Message message = new Message();
        try
        {
            await ws.ReceiveAsync(buffer, CancellationToken.None);
            string str = System.Text.Encoding.Default.GetString(buffer.ToArray());
            JsonUtility.FromJsonOverwrite(str, message);
            Debug.Log("Recieved: " + message);
        }
        catch(Exception e) 
        {
            Debug.Log(e);
        }
        return message;
    }

    private async void checkStatus() {
        if(ws.State != WebSocketState.Open) {
            await Connect();
        }
    }
}



