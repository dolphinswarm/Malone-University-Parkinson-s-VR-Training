#pragma strict
var mainCam : Transform;
var pitchMin : float = -40;
var pitchMax : float = 20;

function Start () {
    if (mainCam == null){
        if (GameObject.Find("CenterEyeAnchor") != null){
            //Debug.Log ("Staying in front of mainCam: CenterEyeAnchor");
            mainCam = GameObject.Find("CenterEyeAnchor").transform;  // was "Main Camera"
        }
        else if (GameObject.Find("Main Camera") != null){
            //Debug.Log ("Staying in front of mainCam: Main Camera");
            mainCam = GameObject.Find("Main Camera").transform;  // was "Main Camera"
        }
    }
}

function Update () {
    transform.eulerAngles.y = mainCam.eulerAngles.y;
    if(mainCam.eulerAngles.x > pitchMin && mainCam.eulerAngles.x < pitchMax)
        transform.eulerAngles.x = mainCam.eulerAngles.x;
}