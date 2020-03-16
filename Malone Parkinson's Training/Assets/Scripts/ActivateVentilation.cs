using UnityEngine;
using System.Collections;

public class ActivateVentilation : MonoBehaviour {

	public GameObject ventUI;
    bool hasRun = false;

    // Use this for initialization
    void Start () {
		ventUI = GameObject.Find("VentilationToggle").transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		

        if (!hasRun)
        {
                 if (gameObject.GetComponent<InfoObject>() != null)
                {
                    if (gameObject.GetComponent<InfoObject>().isActive)
                    {
                        vent();
                    }
                }
                else if (gameObject.GetComponent<QuestionEvent>() != null)
                {
                    if (gameObject.GetComponent<QuestionEvent>().isActive)
                    {
                        vent();
                    }
                }
              
		}
	}

    void vent()
    {
        // turn on ventilation UI
        ventUI.SetActive(true);
        // reset and start
        ventUI.GetComponent<Ventilation>().Reset();
        ventUI.GetComponent<Ventilation>().Go();
        hasRun = true;
    }
}
