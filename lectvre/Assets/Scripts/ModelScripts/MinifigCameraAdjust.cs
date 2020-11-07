using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinifigCameraAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = gameObject.transform.eulerAngles;
        newRotation.y = Camera.main.transform.eulerAngles.y;
        gameObject.transform.eulerAngles = newRotation;
    }
}
