using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object storing special instructions for objects
/// </summary>
[Serializable]
public class ObjectInstructions
{
    // ======================================================== Variables
    public bool isActive;                   // Should this object be made active or inactive?
    public bool alterPosition = false;      // Should we move this object?
    public Vector3 position;                // ----- If so, here's the new position (local)
    public bool alterRotation = false;      // Should we rotate this object?
    public Vector3 rotation;                // ----- If so, here's the new rotation (local)
    public bool alterScale = false;         // Should we scale this object?
    public Vector3 scale;                   // ----- If so, here's the new scale (local)
    public GameObject gameObjectToAlter;    // The game object to alter.
}

/// <summary>
/// Organizes a room for a given state.
/// </summary>
public class RoomOrganizationEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Game Flow")]
    public SimState nextState;
    public bool alterIV = false;
    public int iVValue = 0;
    public bool alterNGT = false;
    public int nGTValue = 0;

    [Header("Room Set Instructions")]
    public List<ObjectInstructions> objectInstructions;

    [Header("Clock")]
    public Clock clock;
    public Vector2Int clockTime;

    [Header("Clipboard")]
    [TextArea]
    public string titleText;
    [TextArea]
    public string side1Text;
    [TextArea]
    public string side2Text;

    [Header("Player")]
    public bool movePlayer;
    public Vector3 newPlayerPosition;
    public Vector3 newPlayerRotation;

    // ======================================================== Methods
    /// <summary>
    /// On event start, organize the room
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // Go to base event
        base.Go(prevEventNum);

        // Print that the room is being organized
        // Print message to console
        Debug.Log("*** Organizing the Room: Event #" + myEventNum);

        // Set the next state in the game manager
        gameManager.currentState = nextState;

        // Set the room
        foreach (ObjectInstructions inst in objectInstructions)
        {
            // Set active status
            inst.gameObjectToAlter.SetActive(inst.isActive);

            // Set position, if we should
            if (inst.alterPosition) 
                inst.gameObjectToAlter.transform.localPosition = inst.position;

            // Set rotation, if we should
            if (inst.alterRotation)
                inst.gameObjectToAlter.transform.localRotation = Quaternion.Euler(inst.rotation);

            // Set sacle, if we should
            if (inst.alterScale)
                inst.gameObjectToAlter.transform.localScale = inst.scale;
        }

        // Set the clock time
        if (clock != null)
        {
            clock.StopClock();
            clock.SetTime(clockTime.x, clockTime.y);
            clock.StartClock();
        }
            

        // Set the clipboard text
        gameManager.clipboardText.titleText.text = titleText;
        gameManager.clipboardText.colOneText.text = side1Text;
        gameManager.clipboardText.colTwoText.text = side2Text;

        // Move the player, if we should
        if (movePlayer)
        {
            CharacterController charCont = gameManager.currentFPC.GetComponent<CharacterController>();
            if (charCont != null)
            {
                charCont.enabled = false;
                charCont.transform.position = newPlayerPosition;
                charCont.transform.rotation = Quaternion.Euler(newPlayerRotation);
                charCont.enabled = true;
            }
            else
            {
                gameManager.currentFPC.transform.position = newPlayerPosition;
                gameManager.currentFPC.transform.rotation = Quaternion.Euler(newPlayerRotation);
            }
        }

        //  If we want to alter the IV or NGT, do so
        if (alterIV || alterNGT)
        {
            IVScript machine = FindObjectOfType<IVScript>();

            if (alterIV) machine.SetIVValue(iVValue);
            
            if (alterNGT) machine.SetNGTValue(nGTValue);
        }

        // Immediately call finished
        Finished();
    }
}
