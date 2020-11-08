using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputter : MonoBehaviour
{

    public GameObject nameField;
    public GameObject codeField;

    public GameObject leaderNameField;
    public GameObject URLField;

    public void Store() {
        GlobalControl.Instance.name = nameField.GetComponent<Text>().text;
        GlobalControl.Instance.code = codeField.GetComponent<Text>().text;
    }

    public void LeaderStore() {
        GlobalControl.Instance.leaderName = leaderNameField.GetComponent<Text>().text;
        GlobalControl.Instance.URL = URLField.GetComponent<Text>().text;
    }

}
