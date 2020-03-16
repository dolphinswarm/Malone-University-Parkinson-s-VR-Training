/*
	finds all the events in this scene and orders them accordingly
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class eventLoadoutManager : MonoBehaviour {
	
	public Hashtable	eventList = new Hashtable();

	//each bool repersents which role has that event. I.E job1 = monitor technician
	public bool			job1,
						job2,
						job3;
	
	EventManager			currentEvent;
	characterSelectManager	charSelect;

	void Awake() {

		// set this and the player to persist
		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(GameObject.Find("First Person Controller"));
        DontDestroyOnLoad(GameObject.Find("OVRPlayerController"));
    }

	IEnumerator OnLevelWasLoaded() {

		// clear event list to make room for new ones
		eventList.Clear();

		// job management
		charSelect = GameObject.Find("Character Select Manager").GetComponent<characterSelectManager>();
		job1 = charSelect.job1;
		job2 = charSelect.job2;
		job3 = charSelect.job3;
		if(!job1 && !job2 && !job3) Debug.Log("No job chosen.");
        //will somehow have to replace with the value of jobRole in GameSettings???



		
		// add all objects tagged "Event" to eventList
		foreach(GameObject gobj in GameObject.FindGameObjectsWithTag ("Event")) {

			//Debug.Log("gobjE: " + gobj.name);
			EventManager gobjE = gobj.GetComponent<EventManager>();
			//Debug.Log("Job1: " + job1);
			//Debug.Log("Job2: " + job1);
			//Debug.Log("Job3: " + job1);
			//Debug.Log("Event: " + gobjE.order.ToString() + " - " + gobjE.gameObject.name);

			if((job1 && gobjE.job1) || (job2 && gobjE.job2) || (job3 && gobjE.job3)) {
                if (eventList.ContainsKey(gobjE.order.ToString() ))
                {
                    Debug.Log("Duplicate events found at #" + gobjE.order.ToString() + " - duplicate event name is: " + gobjE.gameObject.name);
                } 
                    eventList.Add(gobjE.order.ToString(), gobjE);
				//Debug.Log("Event List Order: " + gobjE.order.ToString() +" - " + eventList[gobjE.order.ToString()].ToString());
			}

			// turn off questions, deliveries, destinations
			if(gobjE.question || gobjE.delivery || gobjE.destination || gobjE.text) gobjE.gameObject.SetActive(false);
		}
		// Activating the smallest hashtable key's object, breaking if the lowest is key is higher than 10000.
		// If the lowest key is over 10000, what are you doing?
		for(int i = 0; i > -1; i++) {
			if(eventList.ContainsKey(i.ToString())) {
				Debug.Log("Activating Event " + eventList[i.ToString()].ToString());
				((EventManager)eventList[i.ToString()]).current = true;
				currentEvent = (EventManager)eventList[i.ToString()];
				currentEvent.gameObject.SetActive(true);
				currentEvent.enabled = true;
				if(currentEvent.text) {
					Debug.Log ("Activating delayText event " + currentEvent.gameObject.name );
					return currentEvent.GetComponent<TimedText>().delayText();
				}
				break;
			}
			if(i > 1000) {
				break;
			}
		}
		return null;
	}
	
	// Update is called once per frame
	void Update() {
		// If there is a current event, and it's completed, then the program moves forward.
		if(currentEvent != null) {
			if(currentEvent.completed == true) {
				StartCoroutine("step"); 
			}
		}
	}
	
	// Step is used to iterate to the next event.
	IEnumerator step() {
		// The current event component is deactivated, and if it is a question, delivery, or 
		// destination event, then the whole game object is deactivated.
		currentEvent.enabled = false;
		if(currentEvent.question || currentEvent.destination) {
			currentEvent.gameObject.SetActive(false);
		}
		
		// THIS COULD BE AN INFINITE LOOP.
		// This loop WILL break if it doesn't complete in a timely (1000 iterations) manner.
		for(int i = currentEvent.order + 1; i > -1; i++) {
			Debug.Log("Iterating - " + i.ToString());
			if(eventList.ContainsKey(i.ToString())) {
				//Debug.Log("hi how ya doin");
				((EventManager)eventList[(i).ToString()]).current = true;
				currentEvent = (EventManager)eventList[i.ToString()];
				currentEvent.gameObject.SetActive(true);
				currentEvent.enabled = true;
				if(currentEvent.text) {
					Debug.Log("Text event " + currentEvent.gameObject.name + " delayText method started.");
					return currentEvent.GetComponent<TimedText>().delayText();
				}
				break;
			}
			
			// If this loop iterates too long, then it is mercilessly killed and an error is thrown.
			if(i - currentEvent.order > 1000) {
				currentEvent.completed = false;
				Debug.Log("FATAL ERROR - iterated event step too many times");
				break;
			}
		}
		return null;
	}
}