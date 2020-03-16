using UnityEngine;
using System.Collections;

public class alwaysShowCursor : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Cursor.visible == false)
			Cursor.visible = true;
	}
}
