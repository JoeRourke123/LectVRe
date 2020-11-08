using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputter : MonoBehaviour
{

    public string name;
    public GameObject nameField;

    public string code;
    public GameObject codeField;

    public void Store() {
        GlobalControl.Instance.name = nameField.GetComponent<Text>().text;
        GlobalControl.Instance.code = codeField.GetComponent<Text>().text;
    }

}
