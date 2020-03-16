using UnityEngine;
using System.Collections;

public class ToggleTimer : MonoBehaviour {

	public GameObject timerObject;
	public enum StartStop {Start, Stop};
	public StartStop timerAction;
	public bool hasToggled = false;

	// Use this for initialization
	void Start () {
        // find timer
        timerObject = GameObject.Find("TimerObject");
        //timerObject = GameObject.Find("Utility_Helper");
        Debug.Log("I found the timerObject: " + timerObject.name);

       

        //what if instead of whatever Utility Helper is err was we make timerObj = gameObject seeing as this script is on the event itself
        //note that there are two evebts with this script on, the start event and the stop.

		/*
		// start or stop as necessary
		if (timerAction == StartStop.Start){
			timerObject.GetComponent<CompletionTimer>().StartTimer();
		}
		if (timerAction == StartStop.Stop){
			timerObject.GetComponent<CompletionTimer>().StopTimer();
		}
		// */
	}

	void Update(){
		if(!hasToggled) { // use the current event system stuff instead of this
                                                                 // make sure we only toggle once
                                                                 // ...and only when the desired event activates
            if (gameObject.GetComponent<InfoObject>() != null)
            {
                if (gameObject.GetComponent<InfoObject>().isActive)
                {
                    timer();
                }
            }else if (gameObject.GetComponent<DestinationEvent>() != null)
            {
                if (gameObject.GetComponent<DestinationEvent>().isActive)
                {
                    timer();
                }
            }
            // start or stop as necessary
            //    if we should start, and it's off, make it go

        }

	}

    void timer()
    {
        if (timerAction == StartStop.Start && !timerObject.GetComponent<CompletionTimer>().timerOn)
        {
            timerObject.GetComponent<CompletionTimer>().StartTimer();
            hasToggled = true;
            Debug.Log(gameObject.name + " is Starting The Timer");
        }
        //    if we should stop, and it's on, make it end
        if (timerAction == StartStop.Stop && timerObject.GetComponent<CompletionTimer>().timerOn)
        {
            timerObject.GetComponent<CompletionTimer>().StopTimer();
            hasToggled = true;
            Debug.Log(gameObject.name + " is stopping The Timer");
        }
    }


}
