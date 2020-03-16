using UnityEngine;
using System.Collections;

public class ShowO2Cart : MonoBehaviour {

	// This script is intended to search the scene, find the (hidden) Oxygen cart on the 1st Person Controller,
	//  and then show it during the evacuation.

	// Note that the 1st person controller carries over from a different scene, so we can't assign evacBasket in inspector... have to search at runtime.
	GameObject o2Cart;
	bool hasRun = false;

	public enum ShowHideSelector { hide, show };
	public ShowHideSelector showHide;

	//GameObject blanket;    // we could modify this script to incrementally show basket, blanket, baby
	//GameObject baby;

	//public bool showBasket = true;
	//public bool showBlanket = false;
	//public bool showBaby = false;



	// Use this for initialization
	void Start() {
		o2Cart = GameObject.Find("OxygenPivot").transform.GetChild(0).gameObject;  // search for the cart node in 1st Person controller
	}

	void Update() {


        if (!hasRun)
        {
            if (gameObject.GetComponent<PickupEventOld>() != null)
            {
                if (gameObject.GetComponent<PickupEventOld>().isActive)
                {
                    Tank();
                }
            }
            else if (gameObject.GetComponent<InfoObject>() != null)
            {
                if (gameObject.GetComponent<InfoObject>().isActive)
                {
                    Tank();
                }
            }
            else if (gameObject.GetComponent<DestinationEvent>() != null)
            {
                if (gameObject.GetComponent<DestinationEvent>().isActive)
                {
                    Tank();
                }
            }
            else if (gameObject.GetComponent<QuestionEvent>() != null)
            {
                if (gameObject.GetComponent<QuestionEvent>().isActive)
                {
                    Tank();
                }
            }
            else if (gameObject.GetComponent<DeliveryEventOld>() != null)
            {
                if (gameObject.GetComponent<DeliveryEventOld>().isActive)
                {
                    Tank();
                }
            }
        }



       
	}

    void Tank()
    {
        hasRun = true;
        o2Cart.SetActive(showHide == ShowHideSelector.show);
    }

}