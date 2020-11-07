using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if(UNITY_2018_3_OR_NEWER)
using UnityEngine.Android;
#endif

using agora_gaming_rtc;

public class AgoraChat : MonoBehaviour
{
    public string AppID = "499ffa8267e545ad995b67464403920b";
    //006499ffa8267e545ad995b67464403920bIAAb8u60d1Rgf18Pj53jLh9GAj2POgnDOW+RrZoRlF3m2YamEDYAAAAAEABqf2ZwAtunXwEAAQAB26df
    public string ChannelName;

    VideoSurface myView;
    VideoSurface remoteView;
    IRtcEngine mRtcEngine;
    #if (UNITY_2018_3_OR_NEWER)
    private ArrayList permissionList = new ArrayList();
    #endif

    void Awake()
    {
        SetupUI();
    }

    void Start()
    {
        #if(UNITY_2018_3_OR_NEWER)
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
        #endif
        SetupAgora();
    }
    private void CheckPermission()
    {
        #if(UNITY_2018_3_OR_NEWER)
        foreach(string permission in permissionList)
        {
            if (Permission.HasUserAuthorizedPermission(permission))
            {
            }
            else
            {
                Permission.RequestUserPermission(permission);
            }
        }
        #endif
    }
    void SetupAgora()
    {
        mRtcEngine = IRtcEngine.GetEngine(AppID);

        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
        Join();
    }

    void SetupUI()
    {
        GameObject go = GameObject.Find("MyView");
        myView = go.AddComponent<VideoSurface>();
        go = GameObject.Find("LeaveButton");
        go?.GetComponent<Button>()?.onClick.AddListener(Leave);
        go = GameObject.Find("JoinButton");
        go?.GetComponent<Button>()?.onClick.AddListener(Join);

        //StartCoroutine(waiter());
        Debug.Log(ChannelName);

    }

    // IEnumerator waiter()
    // {

    //     //Wait for 4 seconds
    //     Debug.Log("Waiting test");
    //     yield return new WaitForSeconds(10);

    //     Join();
    // }

    void Join()
    {
        Debug.Log("Joining test");
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        myView.SetEnable(true);
        mRtcEngine.JoinChannel(ChannelName, "", 0);
    }

    void Leave()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableVideo();
        mRtcEngine.DisableVideoObserver();
    }

    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        #if(UNITY_2018_3_OR_NEWER)
        // Ask for your Android device's permissions.
            CheckPermission();
        #endif
    }

     void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        // can add other logics here, for now just print to the log
        Debug.LogFormat("Joined channel {0} successful, my uid = {1}", channelName, uid);
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        myView.SetEnable(false);
        if (remoteView != null)
        {
            remoteView.SetEnable(false);
        }
    }

    void OnUserJoined(uint uid, int elapsed)
    {
        GameObject go = GameObject.Find("RemoteView");

        if (remoteView == null)
        {
            remoteView = go.AddComponent<VideoSurface>();
        }

        remoteView.SetForUser(uid);
        remoteView.SetEnable(true);
        remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        remoteView.SetGameFps(30);
    }

    void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        remoteView.SetEnable(false);
    }
}
