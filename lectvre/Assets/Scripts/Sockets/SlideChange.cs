using System;

/**
* Used for creating a new room
**/
[Serializable]
public class SlideChange : MainMessage
{
    public int slide;
    public string slideURL;

    public SlideChange(string type, int slide, string slideURL) : base(type){
        this.slide = slide;
        this.slideURL = slideURL;
    }

    public SlideChange(string type, int slide) : base(type)
    {
	    this.slide = slide;
    }

    new public string toJson() {
        return $"{{\"type\":\"{type}\", \"slide\":\"{slide}\"}}";
    }
}
