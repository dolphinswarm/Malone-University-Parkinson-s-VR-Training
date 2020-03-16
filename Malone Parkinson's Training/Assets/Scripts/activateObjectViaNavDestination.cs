using UnityEngine;
using System.Collections;

public class activateObjectViaNavDestination : MonoBehaviour {

	//object to activate
	public GameObject activateThis;
	//npc gameObject
	public GameObject npcObject;

	public bool hasEntered = false;
	// Use this for initialization
	void Start () {
		activateThis.SetActive(false);
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == npcObject.name) {
			//hasEntered = true;
			activateThis.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {
	
		//if()
	}
}
