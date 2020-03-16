using UnityEngine;
using System.Collections;

public class activateNavMeshFollow : MonoBehaviour {
	//have a navmeshmanager object that would hold this script
	// activate the follow navemesh script and deactivates the other navmesh scripts on the specific npc

	private EventManager eventManagerRef;

	public GameObject gameObjectEventOfOccurence; // which event gameobject will activate this the script when it's current
	bool isInjob1; // signifies if the player chose the monitor technician. This script isn't need for the monotor technician
	public testNavMeshFollow scriptToActivate; // the script that we will activate on a specific object.
	bool hasActivated; // makes sure we only activate the script once
					   //^ NOTE: Will need to change the type of this script if we remove "test" from the script name
	public activateNavMeshFollow thisScriptref;

	// reference to this script to disable it once we enable the follow script

	// Use this for initialization
	void Start() {

		eventManagerRef = gameObjectEventOfOccurence.GetComponent<EventManager>();
		isInjob1 = GameObject.FindGameObjectWithTag("CharacterSelectManager").GetComponent<characterSelectManager>().job1;
		thisScriptref = gameObject.GetComponent<activateNavMeshFollow>();
	}

	// Update is called once per frame
	void Update() {

		if(eventManagerRef.current && !hasActivated) { // when this is the current active event
			if(!isInjob1) { // Did player choose job 1? Only makes this check if the event is active.
				scriptToActivate.enabled = true;
				hasActivated = true;
				thisScriptref.enabled = false;
			}
		}
	}
}
