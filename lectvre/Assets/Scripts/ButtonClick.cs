using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{

    private static float TOTAL_TIME = 2;
    private float TIME = 2;
    private bool lookingAt = false;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() { 
        if(lookingAt) {
            TIME -= Time.deltaTime;
            if(TIME < 0) {
                TIME = 0;
            }
            gameObject.GetComponent<Transform>().Find("Text").GetComponent<Text>().text = "CLICKING";
            gameObject.GetComponent<Image>().fillAmount = TIME/TOTAL_TIME;
        }
        if(lookingAt && TIME == 0) {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    void OnPointerEnter() {
        Debug.Log("Hit the button");
        lookingAt = true;

    }

    void OnPointerExit() {
        Debug.Log("Exited the button");
        lookingAt = false;
    }
}
