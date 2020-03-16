using UnityEngine;
using System.Collections;

// the character would keep teleporting through the floor,
//so this script will help set it's Y value to a specifc value to prevent that from happening. 
public class maintainYPositionValue : MonoBehaviour {

	[SerializeField] // just makes it so we still see and edit this vaule in the inspector
	private float startYVal;
	// Use this for initialization
	void Start () {
		//startYVal = gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		// gets the transform scale value, sets the Y poisition and then sets the objects position to the new value
		Vector3 tempScale = gameObject.transform.localPosition;
		tempScale.y = startYVal;
		gameObject.transform.localPosition = tempScale;
	}
}
