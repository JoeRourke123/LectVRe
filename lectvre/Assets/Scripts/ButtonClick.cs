using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{

    private static float TOTAL_TIME = 10;
    private float TIME;
    private bool lookingAt = false;

    public GameObject targetGO;

    public string targetFN;
    // Start is called before the first frame update
    void Start() {
        TIME = TOTAL_TIME;
        gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
    }

    // Update is called once per frame
    void Update() { 
        Debug.Log("Looking At Button: " + lookingAt);
        if(lookingAt) {
            TIME -= Time.deltaTime;
            if(TIME < 0) {
                TIME = 0;
            }
            Debug.Log(Time.deltaTime + " @ " + (255 - Mathf.RoundToInt((TIME/TOTAL_TIME) * 225)));
            gameObject.GetComponent<Renderer>().material.color = new Color(
                255 - Mathf.RoundToInt((TIME/TOTAL_TIME) * 225), 
                255 - Mathf.RoundToInt((TIME/TOTAL_TIME) * 225), 
                255 - Mathf.RoundToInt((TIME/TOTAL_TIME) * 255)
                );
        }
        if(lookingAt && TIME == 0) {
            targetGO.SendMessage(targetFN);
        }
    }

    void OnPointerEnter() {
        Debug.Log("Hit the button");
        lookingAt = true;
    }

    void OnPointerExit() {
        Debug.Log("Exited the button");
        gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        TIME = TOTAL_TIME;
        lookingAt = false;
    }
}
