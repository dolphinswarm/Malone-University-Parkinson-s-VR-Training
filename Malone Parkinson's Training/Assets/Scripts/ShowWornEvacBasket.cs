using UnityEngine;
using System.Collections;

public class ShowWornEvacBasket : MonoBehaviour {

	// This script is intended to search the scene, find the (hidden) evacuation basket on the 1st Person Controller,
	//  and then show it during the evacuation.

	// Note that the 1st person controller carries over from a different scene, so we can't assign evacBasket in inspector... have to search at runtime.
	GameObject evacBasket;
	bool hasRun = false;

	//GameObject blanket;    // we could modify this script to incrementally show basket, blanket, baby
	//GameObject baby;
	//GameObject vent;

	public bool showBasket = true;
	public bool showBlanket = false;
	public bool showBaby = false;
	public bool showVent = false;
    public bool showWarmer = false;

	private GameObject gameManager;

    public GameObject character; //Character to reference whether or not to set active basket stuff

    // Use this for initialization
    void Start () {
		evacBasket = GameObject.Find("EvacBasket Pivot");
		gameManager = GameObject.Find("Game Manager");

        // NEED TO FIND THESE OBJECTS, but this will be hard, b/c they are disabled and in other scene
        //baby = GameObject.Find("EvacBasket Pivot").transform.GetChild(0).gameObject;  
        //vent = GameObject.Find("EvacBasket Pivot").transform.GetChild(0).gameObject;
        // ...added BabyData component to EvacBasket Pivot, with myBaby, myVent, myBlanket, myBasket

        character = GameObject.Find("First Person Controller");
        if (character == null)
        {
            character = GameObject.Find("OVRPlayerController");
        }

    }

	void Update() {
        /*if (character == GameObject.Find("First Person Controller"))
        {

        }else
        {*/
            if (!hasRun)
            {
            if(gameObject.GetComponent<PickupEventOld>() != null)
            {
                if (gameObject.GetComponent<PickupEventOld>().isActive)
                {
                    Basket();
                }
            }
            else if (gameObject.GetComponent<InfoObject>() != null)
            {
                if(gameObject.GetComponent<InfoObject>().isActive)
                {
                    Basket();
                }
            }
             else if (gameObject.GetComponent<QuestionEvent>() != null)
            {
                if (gameObject.GetComponent<QuestionEvent>().isActive)
                {
                    Basket();
                }
            }

        }
        //}

    }

    void Basket()
    {
        hasRun = true;

        if (showBasket) { evacBasket.GetComponent<BabyData>().myBasket.SetActive(true); }

        if (showBaby) {
            evacBasket.GetComponent<BabyData>().myBaby.SetActive(true);

            // hide vent tube for Job 1 (monitor tech)... this is probably not good to hard-code
            //if (gameManager.GetComponent<GameSettings>().jobRole == JobRole.MonitorTech)//(gameManager.GetComponent<eventLoadoutManager>().job1 == true)
              
            //{
                
            //    evacBasket.GetComponent<BabyData>().myTube.SetActive(false);

            //}
        }
        //if (showVent) { evacBasket.GetComponent<BabyData>().myVent.SetActive(true); }
        //if (showBlanket) { evacBasket.GetComponent<BabyData>().myBlanket.SetActive(true); }
        //if (showWarmer) { evacBasket.GetComponent<BabyData>().myWarmer.SetActive(true); }
       
        
        
    }




}
