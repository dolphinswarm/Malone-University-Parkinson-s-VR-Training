using UnityEngine;
using System.Collections;

public class CursorHit : MonoBehaviour {

    //public HeadLookControllerModified headLook;
    // array of instances of the headlook controller modified scripts in scene, then stores as array of the instances of the script
    public HeadLookControllerModified[] headLooks;// = FindObjectsOfType(typeof(HeadLookControllerModified)) as HeadLookControllerModified[];
    private float offset = 1.5f;
    public float speed = 2;
    //public int levelLoadedOn;  // finds the new instances of the head look scrit when loading a new scene

    void Start()
    {
        headLooks = FindObjectsOfType(typeof(HeadLookControllerModified)) as HeadLookControllerModified[];
    }

    void OnLevelWasLoaded(int level)
    {
        //if(level == 4)
            headLooks = FindObjectsOfType(typeof(HeadLookControllerModified)) as HeadLookControllerModified[];
    }

        // Update is called once per frame
        void LateUpdate () {
		if (Input.GetKey(KeyCode.UpArrow))
			offset += Time.deltaTime;
		if (Input.GetKey(KeyCode.DownArrow))
			offset -= Time.deltaTime;

        /*Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(cursorRay, out hit)) {
			transform.position = hit.point + offset * Vector3.up;
		}
        

        if (Input.GetKeyDown("w"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + speed);
        }

        else if (Input.GetKeyDown("s"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - speed);
        }

        else if (Input.GetKeyDown("a"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - speed);
        }

        else if (Input.GetKeyDown("f"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - speed);
        }
        */
        //sets this object to the target of eac of the head look scripts
        foreach (HeadLookControllerModified headLook in headLooks)
        {
            headLook.target = transform.position;
        }
	}
}
