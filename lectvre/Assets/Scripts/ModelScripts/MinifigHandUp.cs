using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinifigHandUp : MonoBehaviour
{
    public Animation raiseHand;
    bool isHandRaised;
    // Start is called before the first frame update
    void Start()
    {
        bool isHandRaised = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHandRaised) {
            //run animation
        } 

        else {
            //make sure hand is down
        }
        

    }
}
