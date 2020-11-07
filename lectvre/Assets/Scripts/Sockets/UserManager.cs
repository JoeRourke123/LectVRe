using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserManager : MonoBehaviour
{

    private Dictionary<string, GameObject> users = new Dictionary<string, GameObject>();
    public GameObject userObject;
    public string userId = null;

    // Start is called before the first frame update
    async void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnJoin(string userId) {
        this.userId = userId;
    }

    void UpdateUsers(RecMessage message) {
        if(message.id != userId) {
            if(users.ContainsKey(message.id)) {
                GameObject user = users[message.id];
                user.transform.position = message.toVector3();

                Vector3 newRotation = user.transform.eulerAngles;
                newRotation.y = message.getAngle();
                user.transform.eulerAngles = newRotation;
            }
            else {
                users.Add(message.id, Instantiate(userObject, message.toVector3(), message.toQuaternion(), gameObject.transform));
            }
        }
    }
}



