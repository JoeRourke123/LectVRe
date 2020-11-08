using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomiseColour : MonoBehaviour
{

    public Material[] skinColour;
    public Material[] hairColour;
    public Material[] colour;
    
    private int current;

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
