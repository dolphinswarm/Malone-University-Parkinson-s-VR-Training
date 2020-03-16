using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointConstraint : MonoBehaviour {

    public Transform objToFollow;

    // Use this for initialization
    void Start () {
       	
	}



    // Update is called once per frame
    void Update() {
        //zDist = objToFollow.localPosition.z - transform.localPosition.z;
        transform.localPosition = new Vector3(objToFollow.localPosition.x,
            transform.localPosition.y,
            objToFollow.localPosition.z);
    }

    
}
