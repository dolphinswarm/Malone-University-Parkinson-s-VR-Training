using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestFireAlarmLight : MonoBehaviour {

    public bool FireLight=false;

    Animator animator;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
        ActivateFireAlarm(false);
    }

    public void ActivateFireAlarm (bool activeTF)
    {
        FireLight = activeTF;
        animator.enabled = FireLight;

        // call externally with --> BroadcastMessage("ActivateFireAlarm", true, SendMessageOptions.DontRequireReceiver);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ActivateFireAlarm(!FireLight);
        }

    }
}
