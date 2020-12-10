using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRHapticsManager : MonoBehaviour
{
    // ======================================================== Variables
    public GameManager gameManager;
    public bool leftHand;

    // ======================================================== Methods
    // Start is called before the first frame update
    void Start()
    {
        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager

        // Set the hand in the game manager
        if (gameManager != null)
        {
            if (leftHand)
                gameManager.leftHandOVRHaptics = this;
            else
                gameManager.rightHandOVRHaptics = this;
        }
        else
        {
            if (leftHand)
                FindObjectOfType<TitleScreenManager>().leftHandOVRHaptics = this;
            else
                FindObjectOfType<TitleScreenManager>().rightHandOVRHaptics = this;
        }

    }

    /// <summary>
    /// On frame update, check if buttons are pressed to apply short haptics.
    /// </summary>
    void Update()
    {
        // Apply short haptics for right, if clicked
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            StartCoroutine(OVRHaptics(0.01f, 0.2f, 0.2f, OVRInput.Controller.RTouch));
        }
        // Apply short haptics for left, if clicked
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            StartCoroutine(OVRHaptics(0.01f, 0.2f, 0.2f, OVRInput.Controller.LTouch));
        }
    }

    /// <summary>
    /// Returns the proper controller to vibrate.
    /// </summary>
    /// <returns></returns>
    private OVRInput.Controller GetController()
    {
        if (leftHand) 
            return OVRInput.Controller.LTouch;

        return OVRInput.Controller.RTouch;
    }

    /// <summary>
    /// If we intersect with another collider...
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        // If the other object has a OVR Haptics Trigger, trigger the vibration coroutine
        if (other.gameObject.TryGetComponent<OVRHapticsTrigger>(out OVRHapticsTrigger oVRHapticsTrigger))
        {
            StartCoroutine(OVRHaptics(0.01f, 0.2f, 0.2f, GetController()));
        }
    }

    

    /// <summary>
    /// A coroutine for playing OVR haptics.
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="frequency"></param>
    /// <param name="amplitude"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    IEnumerator OVRHaptics(float seconds, float frequency, float amplitude, OVRInput.Controller controller)
    {
        // Set the OVR haptics
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        // Wait x seconds
        yield return new WaitForSeconds(seconds);

        // Set the OVR haptics
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
