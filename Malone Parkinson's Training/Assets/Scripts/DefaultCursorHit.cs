using UnityEngine;
using System.Collections;

public class DefaultCursorHit : MonoBehaviour {
	
	public HeadLookControllerModified headLook;
	private float offset = 1.5f;
    public float speed = 2;

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
        headLook.targetDefault = transform.position;
	}
}
