using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{   
    public void duplicate() {
        GameObject instance = GameObject.Instantiate(gameObject) as GameObject;
    }
}
