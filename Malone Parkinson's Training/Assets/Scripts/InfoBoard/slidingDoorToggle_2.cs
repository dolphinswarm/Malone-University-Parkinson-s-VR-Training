using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum DoorState { open, closed, opening, closing };


[RequireComponent(typeof(AudioSource))]
public class slidingDoorToggle_2 : MonoBehaviour {
    //Togglable sliding door

    // pointers to other objects
    private AudioSource myAudio;    // points to Audio component of this game object
    public GameObject DoorToMove;   //  door to be moved
    public GameObject text;         // billboard-effect text prompt that floats on the door
    public Transform openPos;       // where the door opens to
    public Transform closedPos;     // where the door closes to

    // Internal state variables
    private bool moving = false;    //flag for Update function to do movement
    public DoorState state = DoorState.closed;  // public so it can be querried for events

    // settings
    public float tolerance = 0.1f;
    public float openCloseTime = 2.0f;

    // Use this for initialization
    void Start() {
        ShowText(false);  // hide text to start
        myAudio = this.gameObject.GetComponent<AudioSource>();  // get handle to audio source
    }


    void Update() {
        // only move the door if the "moving" flag has been set to true (done elsewhere)
        if (moving) {
            // Sound effect alt method: If the audio source isn't already playing, then start.  
            //if (!myAudio.isPlaying) {
            //    myAudio.Play(); // would playoneshot be better?
            //}

            // Move the door here
            // if the door was open or is closing, work to close it
            if (state == DoorState.open || state == DoorState.closing)
            {
                state = DoorState.closing; // make note of state each frame
                DoorToMove.transform.position = Vector3.Lerp(DoorToMove.transform.position, closedPos.position, openCloseTime * Time.deltaTime);
                if (Vector3.Distance(closedPos.position, DoorToMove.transform.position) <= tolerance)
                {
                    //print ("door is closed");
                    moving = false;
                    state = DoorState.closed; // final state of action
                }
            }
            else
            { //state == was closed or is opening... so work towards opening it
                state = DoorState.opening; // make note of state each frame
                DoorToMove.transform.position = Vector3.Lerp(DoorToMove.transform.position, openPos.position, openCloseTime * Time.deltaTime);
                //print ("door is opening");
                if (Vector3.Distance(DoorToMove.transform.position, openPos.position) <= tolerance) {
                    //print("door is open");
                    moving = false;
                    state = DoorState.open; // final state final state of action
                }
            }
        }


    }

    // PERHAPS THIS SHOULD JUST DEFINE METHODS TO BE CALLED EXTERNALLY

    public void ShowText(bool onOff) { text.SetActive(onOff); }
    public void InteractWithDoor() {
        // Play the audio effect
        //  Restart audio clip if it's already playing... e.g., if we're interrupting an open to close
        if (myAudio.isPlaying) { myAudio.Stop(); }
        //myAudio.PlayOneShot(myAudio.clip);    // PlayOneShot() requires clip as argument
        myAudio.Play(); // play the clip assigned in the audio source

        // Set the movement direction
        // toggle movement direction each time user interacts / clicks
        // - if door was open or opening, then close it
        if (state == DoorState.open || state == DoorState.opening) {
            state = DoorState.closing;
        }
        else { // - otherwise, if state was closed or closing, then open it
            state = DoorState.opening;
        }

        // tell Update to start executing movement on subsequent frames
        moving = true;
    }


    


}
