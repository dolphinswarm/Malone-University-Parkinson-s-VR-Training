using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRemoveSubmit : MonoBehaviour {

    public GameObject submitButton;


	void Start () {
       
	}
	
	
	void Update () {

        //if this gameObject.isActive
        if (gameObject.GetComponent<QuestionEvent>().isActive)
        {
            submitButton.SetActive(false);
        }
       
    }
}
