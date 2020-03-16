using UnityEngine;
using System.Collections;

public class activateNavMeshAgentDestinationChanger : MonoBehaviour {

	public GameObject objectWithNavMeshScript;
	NavMeshAgentDestinationChanger navMeshRef;

	// Use this for initialization
	void Start () {

		navMeshRef = objectWithNavMeshScript.GetComponent<NavMeshAgentDestinationChanger>();
	}
	
	// Update is called once per frame
	void Update () {

		//testing purposes; disable script at start
		if(navMeshRef != null) {
			if(Input.GetKeyDown("1"))
				navMeshRef.enabled = true;
		}
	}
}
