/*
	skips to [sceneToLoad] when ctrl+alt+shift is pressed
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeveloperSkipScene : MonoBehaviour {

	public string sceneToLoad;

	void Update() {

		// crtl + alt + shift to skip
		if(Input.GetButton("Fire1") && Input.GetButton("Fire2") && Input.GetButton("Fire3")) {
			SceneManager.LoadScene(sceneToLoad);
		}
	}
}
