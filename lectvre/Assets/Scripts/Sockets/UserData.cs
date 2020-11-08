using UnityEngine;
using System;

[Serializable]
class UserData : MonoBehaviour{
    public string username;
    public string id;

    public UserData(string username, string id, GameObject gameObject) {
        this.username = username;
        this.id = id;
    }
}