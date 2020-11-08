using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customisation : MonoBehaviour
{
    
    public GameObject[] hair;
    private int currentHair;

    public Material[] face;
    public GameObject head;
    private int currentFace;

    public Material[] skinColour;
    public Material[] hairColour;
    
    public Material[] colour;
    private int current;

    private int[] data = new int[6];

    public void SwitchHair() {
        if (currentHair == hair.Length - 1) {
            currentHair = 0;
        } else {
            currentHair ++;
        }
        for (int i = 0; i < hair.Length; i++) {
            if (i == currentHair) {
                hair[i].SetActive(true);
            } else {
                hair[i].SetActive(false);
            }
        }
        data[4] = currentHair;
    }

    public void SwitchHairBack() {
        if (currentHair == 0) {
            currentHair = hair.Length - 1;
        } else {
            currentHair --;
        }
        for (int i = 0; i < hair.Length; i++) {
            if (i ==  currentHair) {
                hair[i].SetActive(true);
            } else {
                hair[i].SetActive(false);
            }
        }
        data[4] = currentHair;
    }

    public void SwitchFace() {
        if (currentFace == face.Length - 1) {
            currentFace = 0;
        } else {
            currentFace ++;
        }
        head.GetComponent<Renderer>().material = face[currentFace];
        data[5] = currentFace;
    }

    public void SwitchFaceBack() {
        if (currentFace == 0) {
            currentFace = face.Length - 1;
        } else {
            currentFace --;
        }
        head.GetComponent<Renderer>().material = face[currentFace];
        data[5] = currentFace;
    }

    public void Switch(string part) {
        if (current == colour.Length - 1) {
            current = 0;
            ChangeColour(current, part);
        } else {
            current ++;
            ChangeColour(current, part);
        }
    }

    public void SwitchBack(string part) {
        if (current == 0) {
            current = colour.Length - 1;
            ChangeColour(current, part);
        } else {
            current --;
            ChangeColour(current, part);
        }
    }

    public void ChangeColour(int colourIndex, string part) {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == part) {
                component.material = colour[colourIndex];
            }
        }
        if (part == "Torso") {
            data[2] = colourIndex;
        } else if (part == "Legs") {
            data[3] = colourIndex;
        }
    }

    public void ChangeSkinColour(int colourIndex) {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == "Skin") {
                component.material = skinColour[colourIndex];
            }
        }
        data[1] = colourIndex;
    }

    public void ChangeHairColour(int colourIndex) {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == "Hair") {
                component.material = hairColour[colourIndex];
            }
        }
        data[0] = colourIndex;
    }

    public void PrintData() {
        GlobalControl.Instance.data = data;
        for (int i = 0; i < 6; i ++) {
            Debug.Log("Print Data: " + i + " " + GlobalControl.Instance.data[i]);
        }
    }

}
