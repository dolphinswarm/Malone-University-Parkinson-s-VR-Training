#pragma strict

private var visible : boolean = false;   // keep track of whether object should be on or off
var thingsToShow : GameObject[];
var textToShow : GameObject[];
var timeOut : float = 3.0;
var useTimeOut : boolean = true;
private var timeSinceClick : float = 0;
private var timeOutRunning : boolean = false;

function Start () {
	visible = false;
	ToggleVisibility();
	
}

function ToggleVisibility(){
    //Debug.Log("Toggling!");
	for (var i:int=0; i< thingsToShow.Length; i++){
		//thingsToShow[i].GetComponent(MeshRenderer).enabled = visible;   // visible should start as false, make sure helper object is hidden
		thingsToShow[i].SetActive( visible );  // this toggles a parent node to show the children
	}
	
	for (var ii:int=0; ii< textToShow.Length; ii++){
		textToShow[ii].GetComponent(MeshRenderer).enabled = visible;  // also need to hide text
		// note that we can't disable the text node itself, because it has to be active for events to change text
	} 
}


function Update () {

	// This will toggle on when the button is pressed, then off the next time it's released
	//Debug.Log( "Fire 1 is pressed? " + Input.GetButton("Fire1").ToString() );
    //Debug.Log( "Fire 2 is pressed? " + Input.GetButton("Fire2").ToString() );
    //Debug.Log( "Fire 3 is pressed? " + Input.GetButton("Fire3").ToString() );

    if (useTimeOut && timeOutRunning){
        timeSinceClick += Time.deltaTime;
        if (timeSinceClick >= timeOut){
            timeOutRunning = false;   // stop timer
            timeSinceClick = 0;    // zero out timer
            visible = false;    //toggle visibility to opposite value - True / False
    	    ToggleVisibility();
        }
	}


    else if (Input.GetButtonDown("Fire2") && (!timeOutRunning || !useTimeOut)){
	    //Debug.Log("Fire2 Down; Timer Running? " + timeOutRunning.ToString() );
        visible = true;    //toggle visibility on
    	ToggleVisibility();
        timeOutRunning = true;     //start timer
    }
    
    // This will toggle on when the button is pressed, then off the next time it's pressed
    else if (Input.GetButtonUp("Fire2") && !useTimeOut){
        //Debug.Log("Fire2 Released; Timer Running? " + timeOutRunning.ToString() );
    	visible = false;    //toggle visibility to opposite value - True / False
    	ToggleVisibility();
        timeOutRunning = false;   // stop timer... even if not actively being used
	}
}