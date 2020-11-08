using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Threading;

public class WebController : MonoBehaviour {

    public delegate void OnResultRecieved(byte[] result);
    public static OnResultRecieved resultRecieved;

    public string toget;

    class AndroidPluginCallback : AndroidJavaProxy {
        public AndroidPluginCallback() : base("com.example.matthew.webViewPlugin.PluginCallback") { }
        public WebController webController;

        public void onFrameUpdate(AndroidJavaObject bytesObj) {
            Debug.Log("Callback onSuccess From Unity!");

            AndroidJavaObject bufferObject = bytesObj.Get<AndroidJavaObject>("Buffer");
            byte[] bytes = AndroidJNIHelper.ConvertFromJNIArray<byte[]>(bufferObject.GetRawObject());

            webController.LoadImageRoutine(bytes);
        }
    }

    AndroidJavaObject activity;
    AndroidJavaObject plugin;
    AndroidJavaObject data;

    bool shouldLoadMainScreen = true;

    private void Start() {
        UnityThread.initUnityThread();
        InitPlugin();
    }

    public void SetLoadMainScreen(bool shoulLoadMan) {
        shouldLoadMainScreen = shoulLoadMan;
    }

    void InitPlugin() {

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

	    plugin = new AndroidJavaObject("com.example.matthew.webViewPlugin.WebBridge");
	    plugin.Call("init");

        //set callback
        AndroidPluginCallback androidPluginCallback = new AndroidPluginCallback {webController = this};
        plugin.Call("SetUnityBitmapCallback", androidPluginCallback);
    }

    void LoadImageRoutine(byte[] bytes) {
        resultRecieved?.Invoke(bytes);
    }

    public void GetImageFromURL(string url) {
        // Calls the function from the jar file
        toget = url;
        Thread thread = new Thread(ThreadedGet);
        thread.Start();
    }

    public void ThreadedGet()
    {
	    plugin.Call("GetBitmap", toget);
    }
}
