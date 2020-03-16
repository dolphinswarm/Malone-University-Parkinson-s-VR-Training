using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StayOnTop : MonoBehaviour {

    public Transform stayOnTopOf;
    public float constantOffset = .85f;
    public bool positionInRealTime = false;

    void AutoSetPosition() {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            stayOnTopOf.localScale.y * constantOffset,
            transform.localPosition.z
            );
    }

	// Use this for initialization
	void Start () {
        // probably better to just move this once... reference object won't normally change scale
        AutoSetPosition();
    }
	
	// Update is called once per frame
	void Update () {
        // test dynamic positioning in editor if requested
        if (positionInRealTime) { AutoSetPosition(); }
    }
}
