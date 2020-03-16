using UnityEngine;
using System.Collections;

public class RenderOnTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.renderQueue = 4000;
		//Debug.Log("--------------Reticle RenderCue now = " + GetComponent<Renderer>().material.renderQueue.ToString());
	}
	
}
