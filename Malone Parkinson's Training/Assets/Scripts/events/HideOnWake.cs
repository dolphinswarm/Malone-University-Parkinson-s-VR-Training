using UnityEngine;
using System.Collections;

public class HideOnWake : MonoBehaviour {

	public GameObject hide;
	
	// Use this for initialization
	void Start () {
		hide.SetActive(false);
	}
	

}
