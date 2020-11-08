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

    public void SwitchHair() {
        if (currentHair == hair.Length - 1) {
            currentHair = 0;
        } else {
            currentHair ++;
        }
        for (int i = 0; i < hair.Length; i++) {
            if (i ==  currentHair) {
                hair[i].SetActive(true);
            } else {
                hair[i].SetActive(false);
            }
        }
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
    }

    public void SwitchFace() {
        if (currentFace == face.Length - 1) {
            currentFace = 0;
        } else {
            currentFace ++;
        }
        head.GetComponent<Renderer>().material = face[currentFace];
    }

    public void SwitchFaceBack() {
        if (currentFace == 0) {
            currentFace = face.Length - 1;
        } else {
            currentFace --;
        }
        head.GetComponent<Renderer>().material = face[currentFace];
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
    }

    public void ChangeSkinColour(int colourIndex) {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == "Skin") {
                component.material = skinColour[colourIndex];
            }
        }
    }

    public void ChangeHairColour(int colourIndex) {
        Renderer[] components = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer component in components) {
            if(component.tag == "Hair") {
                component.material = hairColour[colourIndex];
            }
        }
    }

}
