﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Jobs;

using com.youvisio;

public class LoadImage : MonoBehaviour {
    public Renderer render;
    public string url;

    private bool hasGot = false;
    //
    // public void Update()
    // {
	   //  if (!hasGot)
	   //  {
		  //   StartCoroutine(getImg());
		  //   hasGot = true;
	   //  }
    // }

    public IEnumerator getImg (int slide, string slideURL)
    {
	    string pls = $"http://192.168.0.24:8000/notes?slide={slide}&toget={HttpUtility.UrlEncode(slideURL)}";
	    Debug.Log(pls);
	    WWW www = new WWW(pls);
	    while (!www.isDone)
		    yield return null;
	    Debug.Log (www.texture.name);
	    render.material.mainTexture = www.texture;
    }
}
