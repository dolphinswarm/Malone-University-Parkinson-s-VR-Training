using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_npc_avoidance : MonoBehaviour {


    //GameObject withwhich target/current destination is attached (should be the same as what this script is on)
    public GameObject self;

    //public Transform target;
    //Transform target;
    Vector3 target;

    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //target = I dont remember how to get a variable from another script
        //Declare target to = the current destination 
        target = self.GetComponent<PedestrianTest>().currentDestination;


                            //.position (when using transform version of target)
        //the direction vector leading to target
        Vector3 dir = (target - transform.position).normalized;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        //Check for forward raycast
        if(Physics.Raycast(transform.position, fwd, out hit, 3))
        {
            if(hit.transform != transform && hit.transform.tag == "NPC")
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                dir += hit.normal * 10; //adding direction of the hit face * the force you want to apply (should make that into a public int later so can play with it)
            }
        }


        //Vector3 LeftR = transform.position;
        //Vector3 RightR = transform.position;

       // LeftR.x -= 1;
        //RightR.x += 1;

       /* if (Physics.Raycast(LeftR, fwd, out hit, 3))
        {
            if (hit.transform != transform && hit.transform.tag == "NPC")
            {
                Debug.DrawLine(LeftR, hit.point, Color.red);
                dir += hit.normal * 10; //adding direction of the hit face * the force you want to apply (should make that into a public int later so can play with it)
            }
        }
        */
      /*  if (Physics.Raycast(RightR, fwd, out hit, 3))
        {
            if (hit.transform != transform && hit.transform.tag == "NPC")
            {
                Debug.DrawLine(RightR, hit.point, Color.red);
                dir += hit.normal * 10; //adding direction of the hit face * the force you want to apply (should make that into a public int later so can play with it)
            }
        }
        */

        Quaternion rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
        //transform.position += transform.forward * 20 * Time.deltaTime;
	}

   /* void OnTriggerEnter(Collider other)
    {
        if(other.tag == "npc")
        {

        }
    }*/
}
