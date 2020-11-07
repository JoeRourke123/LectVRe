using UnityEngine;
using System;

[Serializable]
class UserData {
    public string name;
    public GameObject gameObject;
    public string id;

    public UserData(string name, string id, GameObject gameObject) {
        this.name = name;
        this.id = id;
        this.gameObject = gameObject;
    }
}