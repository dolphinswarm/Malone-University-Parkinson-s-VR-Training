using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ventilation : MonoBehaviour {

	public float ventRate = 3.0f;
	public float offTimeThreshold = 1.0f;

	public TextAsset outfile;			// write to this file -- I think textAsset is not correct
	public AudioClip ventClip;			// use this for inflation-bag sound effect

	public AudioClip[] reminderClips;	// audio clip for "remember to do this thing!"
	public bool useReminder = false;	// should reminder clip be played sometimes?
	public int reminderInterval = 10;   // how many misses logged / seconds missed before reminder plays

	//public float minTimeBetweenButtonPress = 0.25f;	// ignore rapidly repeated vents another way

	public bool runOnStartup = true;
	public bool usePractice = true;
	bool amPracticing = false;
	public AudioClip goodJobClip;		// good-job feedback

	public int nPracticeTrials = 5;
	public bool useColor = true;
	public Color baseColor = Color.white;
	public Color goColor = Color.green;
	public Color warningColor = Color.red;
	public Color flashColor = Color.gray;
	public float flashRate = 2.0f;

	public bool useSize = true;
	public float minSize = 0.5f;
	public float maxSize = 1.0f;

	public bool useStartupHelpers = true;
	public int startupCycles = 3;
	public GameObject titleText;
	public GameObject helperText;
	public GameObject visualMarker;

	// these probably shouldn't be public at end, but can be for debugging
	public int nTimesVented = 0;
	public int nTooLate = 0;
	public int nTooEarly = 0;
	public float runningAvg;
	public float maxTime;
	public float minTime;
	public float lastVentTime;
	public float lastVentInterval;
	float leftTriggerValue;
	float rightTriggerValue;
	bool isVenting = false;  // track current state, to avoid repeated inputs
	Material myMat;
	bool running = false;

	private GameObject gameManager;
	private ReportCardManager reportCard;

	// Start runs once at startup
	void Start() {
		myMat = visualMarker.GetComponent<Renderer>().material;   // grab handle to object material
		Reset();


		gameManager = GameObject.Find("Game Manager");
		reportCard = gameManager.GetComponent<ReportCardManager>();

		if (runOnStartup) { Go(); }

	}

	// Use this method to reset the ventilation (e.g., after the tutorial, when actual ventilation starts)
	public void Reset() {
		// would prefer to set nTimesVented to -1 to skip logging the first (likely delayed) ventilation interaction,
		//  but this causes a divide by zero error, which sets the running error to infinity, which causes... um... problems.
		nTimesVented = 0;	// reset to 0 to start over 
		nTooLate = 0;
		nTooEarly = 0;
		runningAvg = ventRate;  // this starting value gets multiplied by zero (and thus wiped out), & replaced with new avg.
		maxTime = ventRate; // anything higher than vent rate becomes new max
		minTime = ventRate; // anything less than vent rate becomes new min
		lastVentTime = Time.time;  // make a note of when we're starting to measure each vent
		titleText.GetComponent<Text>().text = "Ready to Ventilate";
	}

	// Use these to start / stop venting
	public void Go() {
		running = true;
		lastVentTime = Time.time;  // make a note of when we're starting to measure each vent
		if(usePractice) { amPracticing = true; usePractice = false; }
	}

	public void Stop() { running = false; }

	// Use this to log final values to file
	public void LogVentResults() {
		//Debug.Log("Logging ventilation values to text file");
		reportCard.writeLine("Ventilation - nTimesVented = " + nTimesVented.ToString() + "\n");
		reportCard.writeLine("Ventilation - nTooLate = " + nTooLate.ToString() + "\n");
		reportCard.writeLine("Ventilation - nTooEarly = " + nTooEarly.ToString() + "\n");
		reportCard.writeLine("Ventilation - runningAvg = " + runningAvg.ToString() + "\n");
		reportCard.writeLine("Ventilation - maxTime = " + maxTime.ToString() + "\n");
		reportCard.writeLine("Ventilation - minTime = " + minTime.ToString() + "\n");
	}

	// Update is called once per frame
	void Update() {
		// don't do anything unless venting is active
		if(running) { CheckForInput(); }

		// manual start
		if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.V)) {
			Go();
		}
		if(Input.GetKey(KeyCode.RightAlt) && Input.GetKey(KeyCode.V)) {
			Stop();
		}
	}


	void CheckForInput() { 
		leftTriggerValue = Input.GetAxis("xBoxTrigger_L");
		rightTriggerValue = Input.GetAxis("xBoxTrigger_R");
		
		// ignore repeated inputs from axes by using the 'isVenting' flag
		// reset it here when the axes return to zero
		// this shouldn't matter for the keyboard, since it responds to keyDown events only
		// the settings (sensitivity = 100) should force trigger values to be zero or 1
		if (leftTriggerValue == 0 && rightTriggerValue == 0) { isVenting = false; } 

		// respond to venting events here
		if (!isVenting ) {  // only check keyboard and axes if we were not already venting
			if(Input.GetKeyDown(KeyCode.Space) || leftTriggerValue > 0.5 || rightTriggerValue > 0.5) {
				isVenting = true;
				float now = Time.time;
				float myVentTime = now - lastVentTime;
				AddNewTime(myVentTime);
				lastVentTime = now;
				lastVentInterval = myVentTime;
				// reset the color of the visual indicator
				if(useColor) { myMat.color = baseColor; }
				if(useSize) { visualMarker.transform.localScale = new Vector3(minSize, minSize, minSize); }

				if ((useStartupHelpers && nTimesVented > startupCycles) || (!useStartupHelpers)) {
					helperText.active = false;
					titleText.active = false;
				}

				if(useStartupHelpers && (nTimesVented == startupCycles+1)) {
					titleText.active = false;
					helperText.active = false;
				}

				if (amPracticing && nTimesVented == nPracticeTrials) {
					Stop();
					Reset();
					gameObject.GetComponent<AudioSource>().PlayOneShot(goodJobClip);
				}
			}
		}

		// adjust visual size and color here
		AdjustSizeColor();

		// manage startup / helper info
		ManageStartupInfo();
	}



	void AdjustSizeColor() {
		
		if(useColor || useSize) {
			// color should return to base upon venting event
			// otherwise we need to adjust it
			float tempTime = Time.time - lastVentTime;

			// adjust size from vent until ventRate target
			if(useSize && tempTime < ventRate) {
				float sizePct = tempTime / ventRate;
				float newScale = ((maxSize - minSize) * sizePct) + minSize;
				visualMarker.transform.localScale = new Vector3(newScale, newScale, newScale);
			}

			// adjust color from base -> go as we get within threshold of the target
			if(useColor && (ventRate - offTimeThreshold) < tempTime && tempTime < ventRate) {
				float pct = (ventRate - tempTime) / offTimeThreshold;
				myMat.color = Color.Lerp(baseColor, goColor, 1 - pct);  // 1-pct b/c we start 100% away from time & come down to zero


			}
			// adjust color from go -> warning as we go within threshold past the target
			else if(ventRate < tempTime && tempTime < (ventRate + offTimeThreshold)) {
				float pct = (tempTime - ventRate) / offTimeThreshold;
				myMat.color = Color.Lerp(goColor, warningColor, pct);

				// set size to max if necessary
				if(useSize) {
					visualMarker.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
				}

				//helperText.active = true;
				titleText.active = true;
				titleText.GetComponent<Text>().text = "Ventilate Now";
			}

			// flash color from warning - flashColor after we pass the threshold beyond our target
			else if(tempTime > (ventRate + offTimeThreshold)) {
				//Debug.Log("Flashing");
				float pct = Mathf.Repeat (tempTime - (ventRate+offTimeThreshold), offTimeThreshold);
				myMat.color = Color.Lerp(flashColor, warningColor, pct);
				if(useReminder                                      // if we want reminder audio
					&& Mathf.Repeat(tempTime, reminderInterval) >= reminderInterval * .99  // and it's been long enough
					&& !GetComponent<AudioSource>().isPlaying) {    // and the clip isn't already playing
																	// If we get this far, play a reminder
					PlayReminder();
				}
			}
		}
	}

	void PlayReminder() {
		if(useReminder && reminderClips.Length > 0) {
			gameObject.GetComponent<AudioSource>().PlayOneShot(reminderClips[Random.Range(0, reminderClips.Length - 1)]);
			helperText.active = true;
		}
	}


	void ManageStartupInfo() {
		// show startup if requested & within X startup cycles
		if (useStartupHelpers && nTimesVented < startupCycles) {
			titleText.active = true;
			helperText.active = true;

			if((ventRate - (Time.time - lastVentTime))>=0) {
				titleText.GetComponent<Text>().text = "Vent in " + (Mathf.Round((ventRate - (Time.time - lastVentTime)) * 10f) / 10f).ToString();
			}
			else { titleText.GetComponent<Text>().text = "Ventilate Now"; }
		}
		if(useStartupHelpers && nTimesVented == startupCycles) {
			titleText.GetComponent<Text>().text = "Keep it up...";
		}

	}

	// calculate running average
	void AddNewTime (float newVentTime) {
		// increment number of times we've vented
		nTimesVented += 1;
		// update the running average
		//    note that if nTimesVented = 1, whatever default avg is entered will be wiped out (value x 0)
		//    and replaced with newVentTime (i.e., 0 + newVentTime / 1)
		runningAvg = ((runningAvg * (nTimesVented - 1)) + (newVentTime)) / nTimesVented;		

		// check for min/max, regardless of whether vent was on time or not  
		//   (otherwise stats are useless for good venters)
		if(newVentTime > maxTime) { maxTime = newVentTime; } // we set a new record max
		if(newVentTime < minTime) { minTime = newVentTime; } // we set a new record min

		// log if update was too slow / fast
		bool wasVentOnTime = true;
		if (newVentTime >= (ventRate + offTimeThreshold)) {  // vent was too slow
			nTooLate += 1;
			wasVentOnTime = false;
		}
		else if (newVentTime <= (ventRate - offTimeThreshold)) {  // vent was too fast
			nTooEarly += 1;
			wasVentOnTime = false;
		}

		// play reminder sound if necessary
		if (useReminder && !wasVentOnTime) {  // only play if (a) reminders are desired && (b) this vent was missed
			if ((nTooEarly + nTooLate) % reminderInterval == 0) { // use modulus to respond to every X misses
				PlayReminder();
			}
		}


		/*
		// Use this section to check output of stats
		Debug.Log("(" + nTimesVented.ToString() + ") vent time was " + newVentTime.ToString());
		Debug.Log("----- New Vent running Average ------ " + runningAvg.ToString());
		Debug.Log("----- Vent Min ------ " + minTime.ToString());
		Debug.Log("----- Vent Max ------ " + maxTime.ToString());
		Debug.Log("----- Too early ------ " + nTooEarly.ToString());
		Debug.Log("----- Too late ------ " + nTooLate.ToString());
		// */

	}
}
