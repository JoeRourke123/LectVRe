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

}
