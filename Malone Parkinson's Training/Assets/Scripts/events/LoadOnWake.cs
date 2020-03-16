using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnWake : MonoBehaviour {
	
	public string sceneToLoad;

	void Start () {
		SceneManager.LoadScene(sceneToLoad);
	}

}
