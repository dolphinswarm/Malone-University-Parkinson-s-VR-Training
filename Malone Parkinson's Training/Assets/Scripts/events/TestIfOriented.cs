using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestIfOriented : MonoBehaviour {

	public string sceneToLoad;
	
	void Start () {

		/*
			If there is no character select manager in this scene,
			the player has not gone through orientation. So, we
			send him back to pick a job.
		*/
		if(GameObject.Find("Character Select Manager") == null) {
			Debug.Log(Application.loadedLevelName + " loaded, but player has not been orientated.\nPlayer sent back to orientation.");
			SceneManager.LoadScene(sceneToLoad);
		}
	}
}
