using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof (WebController))]
public class TestWebview : MonoBehaviour {

    public Renderer render;
    public string url;

    private Texture2D tex;

    private bool stopped = false;

    WebController webPlugin;

    // Start is called before the first frame update
    void Start() {
        webPlugin = GetComponent<WebController>();
        Thread thread = new Thread(LoadWebsite);
        thread.Start();
    }

    void Update()
    {
	    render.material.mainTexture = tex;
    }

    private void OnEnable() {
        WebController.resultRecieved += GotResult;
    }

    private void OnDisable() {
        WebController.resultRecieved -= GotResult;
    }

    private Texture2D FlipTexture(Texture2D orig) {
	    Texture2D flip = new Texture2D(orig.width,orig.height);
		int xN = orig.width;
	    int yN = orig.height;

	    for(int i=0; i < xN; i++) {
		    for(int j=0; j < yN; j++) {
			    flip.SetPixel(xN-i-1, j, orig.GetPixel(i,j));
		    }
	    }

	    flip.Apply();
	    return flip;
    }

    private void GotResult(byte[] bytes) {
        tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        tex = FlipTexture(tex);
    }

    public void LoadWebsite()
    {
	    while (!stopped)
	    {
		    webPlugin.GetImageFromURL(url);
	    }
    }
}
