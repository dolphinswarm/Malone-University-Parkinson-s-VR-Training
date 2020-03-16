using UnityEngine;
using System.Collections;

public class cribScript : MonoBehaviour
{

    public GameObject crib;
    bool hasRun = false;

    // Use this for initialization
    void Start()
    {
        if (crib == null)
        {
            crib = GameObject.Find("CribPivot").transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasRun)
        {
            if (gameObject.GetComponent<DestinationEvent>() != null) //whatever event type script the event has 
            {
                if (gameObject.GetComponent<DestinationEvent>().isActive)
                {
                    hasRun = true;
                    crib.SetActive(true);
                }
               
            }
        }
    }
}
