using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help_HoldForCue_C : MonoBehaviour {

    private bool visible = false;   // keep track of whether object should be on or off
    
    public GameObject[] thingsToShow;
    public GameObject[] textToShow;
    public float timeOut = 3.0f;
    public bool useTimeOut = true;
    private float timeSinceClick = 0f;
    private bool timeOutRunning = false;

	// Use this for initialization
	void Start () {
        visible = false;
        ToggleVisibility();
    }
	

    void ToggleVisibility() {
        //Debug.Log("Toggling!");
        for (int i = 0; i < thingsToShow.Length; i++){
            //thingsToShow[i].GetComponent(MeshRenderer).enabled = visible;   // visible should start as false, make sure helper object is hidden
            thingsToShow[i].SetActive(visible);  // this toggles a parent node to show the children
        }

        for (int  ii = 0; ii < textToShow.Length; ii++){
            textToShow[ii].GetComponent<MeshRenderer>().enabled = visible;  // also need to hide text
                                                                          // note that we can't disable the text node itself, because it has to be active for events to change text
        }
    }


    // Update is called once per frame
    void Update () {
        if (useTimeOut && timeOutRunning) {
            timeSinceClick += Time.deltaTime;
            if (timeSinceClick >= timeOut) {
                timeOutRunning = false;   // stop timer
                timeSinceClick = 0;    // zero out timer
                visible = false;    //toggle visibility to opposite value - True / False
                ToggleVisibility();
            }
        }


        else if (Input.GetButtonDown("Fire2") && (!timeOutRunning || !useTimeOut)) {
            //Debug.Log("Fire2 Down; Timer Running? " + timeOutRunning.ToString() );
            visible = true;    //toggle visibility on
            ToggleVisibility();
            timeOutRunning = true;     //start timer
        }

        // This will toggle on when the button is pressed, then off the next time it's pressed
        else if (Input.GetButtonUp("Fire2") && !useTimeOut) {
            //Debug.Log("Fire2 Released; Timer Running? " + timeOutRunning.ToString() );
            visible = false;    //toggle visibility to opposite value - True / False
            ToggleVisibility();
            timeOutRunning = false;   // stop timer... even if not actively being used
        }
    }
}
