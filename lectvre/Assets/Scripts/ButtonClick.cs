using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{

    private static int TOTAL_TIME = 2;
    private int TIME = 2;

    private bool lookingAt = false;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() { 
        if(lookingAt) {
            TIME -= Time.timeDelta;
            if(TIME < 0) {TIME = 0;}
        }
        if(lookingAt && TIME == 0) {
            gameObject.GetComponent<Button>().onClick.invoke();
        }
    }

    void OnPointerEnter() {
        lookingAt = true;

    }

    void OnPointerExit() {
        lookingAt = false;
    }
}
