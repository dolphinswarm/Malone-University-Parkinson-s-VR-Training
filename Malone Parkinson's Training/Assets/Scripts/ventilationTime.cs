using UnityEngine;
using System.Collections;

public class ventilationTime : MonoBehaviour {

	public float timeSincePressed = 0f;

	public float hitOnTime = 0f;
	public float hitEarly = 0f;
	public float hitLate = 0f;
	Animator sphereAnim;
	public int numberOfTimesHit = 0;
	public float avgtime;
	//public float avgEarly;
	//public float avgLate;

	// Use this for initialization
	void Start () {

		sphereAnim = GameObject.FindGameObjectWithTag("ventilationSphere").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		timeSincePressed += Time.deltaTime;
		if(timeSincePressed >= 4.5f) {
			sphereAnim.SetBool("reallyLate", true);
		}

		if(Input.GetKeyDown(KeyCode.Space)) 
		{
			numberOfTimesHit++;
			if(timeSincePressed >= 2.5 && timeSincePressed <= 3.5) 
			{
				sphereAnim.SetTrigger("deflate");
				hitOnTime++;
			}

			else if(timeSincePressed <= 2.5) 
			{
				sphereAnim.SetTrigger("deflate");
				hitEarly++;
			}

			else if(timeSincePressed >= 3.5) 
			{
				sphereAnim.SetTrigger("deflate");
				hitLate++;
			}
			calculateRunningAverage();
			timeSincePressed = 0;
			sphereAnim.SetBool("reallyLate", false);
			
		}
	}

	void calculateRunningAverage() {

		/*
		if(timeSincePressed >= 2.5 && timeSincePressed <= 3.5) {
			avgOntime = (((avgOntime * numberOfTimesHit) + timeSincePressed) / (numberOfTimesHit + 1));
		}

		else if(timeSincePressed <= 2.5) {
			sphereAnim.SetTrigger("deflate");
			hitEarly++;
		}

		else if(timeSincePressed >= 3.5) {
			sphereAnim.SetTrigger("deflate");
			hitLate++;
		}
		*/
		avgtime = (((avgtime * numberOfTimesHit) + timeSincePressed) / (numberOfTimesHit + 1));
	}
}
