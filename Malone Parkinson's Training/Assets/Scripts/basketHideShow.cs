using UnityEngine;
using System.Collections;

public class basketHideShow : MonoBehaviour {

	public GameObject[] basketParts;
	bool hasTriggeredOnce = false;
	public bool autoShowWhenCurrent = true;
	EventManager myEvent;

	// Use this for initialization
	void Start () {
		myEvent = GetComponent<EventManager>();

	}
	
	// Update is called once per frame
	void Update () {
		// automatically show when event starts
		if (!hasTriggeredOnce) {
			if(autoShowWhenCurrent && myEvent.current) {
				Show();
				hasTriggeredOnce = true;
			}
		}
	
	}


	void Show() {
		foreach (GameObject g in basketParts) {
			g.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	void Hide() {
		foreach(GameObject g in basketParts) {
			g.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
