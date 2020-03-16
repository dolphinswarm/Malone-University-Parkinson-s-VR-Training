using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOver : MonoBehaviour {

    public string sceneToLoad;

	// Use this for initialization
	void Start () {
        if (GameObject.Find("First Person Controller") == null)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
	}
	

}
