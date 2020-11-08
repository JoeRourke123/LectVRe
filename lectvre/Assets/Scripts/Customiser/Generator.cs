using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{   

    private int[] data  = new int[6];

    public Material[] skinColour;
    public Material[] hairColour;
    public Material[] colour;

    public Material[] face;
    public GameObject head;
    public GameObject[] hair;

    //public void duplicate() {
        //GameObject instance = GameObject.Instantiate(gameObject) as GameObject;
    //}

    public void Change() {
        data = GlobalControl.Instance.data;
        ChangeHair();
        ChangeFace();
        ChangeColour("Torso");
        ChangeColour("Legs");
        ChangeSkinColour();
        ChangeHairColour();
    }

    private void ChangeHair() {
        for (int i = 0; i < hair.Length; i++) {
            if (i == data[4]) {
                hair[i].SetActive(true);
            } else {
                hair[i].SetActive(false);
            }
        }
    }

    private void ChangeFace() {
        head.GetComponent<Renderer>().material = face[data[5]];
    }

    private void ChangeColour(string part) {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == part) {
                if (part == "Torso") {
                    component.material = colour[data[2]];
                } else if (part == "Legs") {
                    component.material = colour[data[3]];
                }
            }
        }
    }

    private void ChangeSkinColour() {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == "Skin") {
                component.material = skinColour[data[1]];
            }
        }
    }

    private void ChangeHairColour() {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == "Hair") {
                component.material = hairColour[data[0]];
            }
        }
    }

}
