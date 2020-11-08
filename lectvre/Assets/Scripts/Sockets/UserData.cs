using UnityEngine;
using System;

/**
* Used for holding user data in the game and used for receiving when a player leaves
**/
[Serializable]
class UserData : MonoBehaviour, IMessageInterface{
    public string username;
    public string user;
    public string type;
    public int seat;
    public UserData(string type, string username, string user, int seat, GameObject gameObject) {  
        this.seat = seat;
        this.type = type;
        this.username = username;
        this.user = user;
    }

    public UserData(string type, string username, string user, GameObject gameObject) {  
        this.type = type;
        this.username = username;
        this.user = user;
    }
    public UserData(string username, string user, GameObject gameObject) {
        this.username = username;
        this.user = user;
    }

    public string toJson() {
        return $"{{\"type\":\"{type}\",\"username\":\"{username}\", \"user\":\"{user}\"}}";
    }
}