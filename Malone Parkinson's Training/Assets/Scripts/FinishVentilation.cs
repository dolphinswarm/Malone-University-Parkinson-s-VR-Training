using UnityEngine;
using System.Collections;

public class FinishVentilation : MonoBehaviour {

	public GameObject ventUI;
	bool hasRun = false;

	// Use this for initialization
	void Start () {
		ventUI = GameObject.Find("VentilationToggle").transform.GetChild(0).gameObject;
		Debug.Log("I found " + ventUI.name);
	}

	// Update is called once per frame
	void Update() {

		if(!hasRun && this.gameObject.GetComponent<EventManager>().current) {
			// stop ventilation system
			ventUI.GetComponent<Ventilation>().Stop();
			// log results
			ventUI.GetComponent<Ventilation>().LogVentResults();

			// turn off ventilation UI
			ventUI.active = false;
			hasRun = true;
		}
	}
}
