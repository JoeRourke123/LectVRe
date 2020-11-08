using UnityEngine;
using System;

[Serializable]
class UserData : MonoBehaviour{
    public string name;
    public string id;

    public UserData(string name, string id, GameObject gameObject) {
        this.name = name;
        this.id = id;
    }
}