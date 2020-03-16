using UnityEngine;
using System.Collections;

public class CompletionTimer : MonoBehaviour {

	public float timeSoFar = 0;
	public bool timerOn = false;


	
	// Update is called once per frame
	void Update () {
		if (timerOn){
			timeSoFar += Time.deltaTime;
		}

        
	}

	public void StartTimer() {
		Debug.Log("Starting Timer");
		timerOn = true;
	}

	public void StopTimer(){
		Debug.Log("Stopping Timer at " + timeSoFar.ToString());
		timerOn = false;
		gameObject.GetComponent<DataStorage>().timeToCompletion = timeSoFar;
	}

    public void ResetTimer()
    {
        timeSoFar = 0;
    }
}
