using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void OnMouseEnter() {
		print(gameObject.name + "got hit");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
