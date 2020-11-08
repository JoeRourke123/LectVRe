using System;

/**
* Class for containing data for joining a room
**/
[Serializable]
public class UserMessage : MainMessage
{
    public string roomId;
    public string username;
    public string user;
    public int seat;
    public MinifigureData minifigureData;

    public int slide;
    public string slideURL;

    public UserMessage(string type, string roomId, string username, string user, MinifigureData minifigureData) : base(type) {
        this.roomId = roomId;
        this.username = username;
        this.user = user;
        this.minifigureData = minifigureData;
    }
    public UserMessage(string type, string roomId, string username, string user, int seat, MinifigureData minifigureData) : base(type) {
        this.roomId = roomId;
        this.username = username;
        this.user = user;
        this.seat = seat;
        this.minifigureData = minifigureData;
    }
    public UserMessage(string type, string roomId, string username, string user, int head, int torso, int legs, int hair, int hairColour, int skinColour) : base(type){
        this.roomId = roomId;
        this.username = username;
        this.user = user;
        this.minifigureData = new MinifigureData(hair, hairColour, skinColour, head, torso, legs);
    }

    public UserMessage(string type, string roomId, string username, string slideURL, int slide, string user, MinifigureData minifigureData) : base(type) {
	    this.roomId = roomId;
	    this.username = username;
	    this.user = user;
	    this.minifigureData = minifigureData;
	    this.slide = slide;
	    this.slideURL = slideURL;
    }
    public UserMessage(string type, string roomId, string username, string slideURL, int slide, string user, int seat, MinifigureData minifigureData) : base(type) {
	    this.roomId = roomId;
	    this.username = username;
	    this.user = user;
	    this.seat = seat;
	    this.minifigureData = minifigureData;
	    this.slideURL = slideURL;
	    this.slide = slide;
    }
    public UserMessage(string type, string roomId, string username, string slideURL, int slide, string user, int head, int torso, int legs, int hair, int hairColour, int skinColour) : base(type){
	    this.roomId = roomId;
	    this.username = username;
	    this.user = user;
	    this.minifigureData = new MinifigureData(hair, hairColour, skinColour, head, torso, legs);
	    this.slideURL = slideURL;
	    this.slide = slide;
    }

    public UserMessage() {

    }

    new public string toJson() {
        return $"{{\"type\":\"{type}\", \"minifig\":{minifigureData.toJson()}, \"user\": \"{ user }\", \"name\":\"{username}\", \"room\":\"{roomId}\"}}";
    }
}
