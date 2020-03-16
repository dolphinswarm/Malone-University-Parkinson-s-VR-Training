using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateVentilation : MonoBehaviour {

    public GameObject ventUI;
    bool hasRun = false;

    // Use this for initialization
    void Start () {
        ventUI = GameObject.Find("VentilationToggle").transform.GetChild(0).gameObject;
       
    }
	
	// Update is called once per frame
	void Update () {

        if(ventUI == null)
        {
            ventUI = GameObject.Find("VentilationToggle").transform.GetChild(0).gameObject;
        }

		if(ventUI && gameObject.GetComponent<InfoObject>().isActive)
        {
            ventUI.SetActive(false);
            // reset but Don start
            ventUI.GetComponent<Ventilation>().Reset();
            //ventUI.GetComponent<Ventilation>().Go();
        }
    }
}
