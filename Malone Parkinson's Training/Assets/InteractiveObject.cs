using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enumeration to determine the type of interactive object.
/// </summary>
public enum ObjectType { PICKUP, TOUCH };

/// <summary>
/// An enumeration to determine the type of interactive object.
/// </summary>
public enum HighlightType { BODY_DISTANCE, HAND_DISTANCE, LOOKAT, POINTAT };

/// <summary>
/// The class for a generic interactive object.
/// </summary>
public class InteractiveObject : Interactive
{
    // ======================================================== Variables
    [Header("Interactive Object Properties")]
    public InfoBoardEvent owningEvent;          // The object this script is calling back to.
    private bool oneClick = false;              // Can this object only be clicked once?
    public ObjectType objectType;               // The type of interactive object

    [Header("Object Hightlight")]
    public Material defaultMaterial;            // The default material of this object.
    public Material highlightMaterial;          // The highlight material of this object.
    public HighlightType highlightType;         // How should this object be highlit?
    private Transform cameraPos;
    [Range(0.0f, 25.0f)]
    public float highlightDistance = 10.0f;     // The highlight distance

    // ======================================================== Methods
    /// <summary>
    /// Initialize this pickup object.
    /// </summary>
    protected override void Initialize()
    {
        // Set owning event, if null
        if (owningEvent == null)
            owningEvent = GetComponentInParent<InfoBoardEvent>();

        // Set info board and destination trigger
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>();

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager

        // Set the default shader
        defaultMaterial = gameObject.GetComponent<Renderer>().material;

        // If highlight shader is not set, get the game manager's
        if (highlightMaterial == null)
            highlightMaterial = gameManager.hightlightMaterial;

        // Get the current FPC camera
        cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // Set the proper looking tag
        if (gameManager.controlType == ControlType.OCULUS)
        {
            requiredTag = "OVR Grabber";
        }
        else
        {
            requiredTag = "Reticle";
        }
    }

    /// <summary>
    /// On frame update...
    /// </summary>
    protected void Update()
    {
        // Get the camera and hand vectors
        Vector3 cameraVec = transform.position - cameraPos.position;
        Vector3 handVec = transform.position - gameManager.currentHand.transform.position;

        // If distance, check it via the distance
        if (highlightType == HighlightType.BODY_DISTANCE && cameraVec.magnitude < highlightDistance)
        {
            Highlight();
        }
        // If distance, check it via the distance
        else if (highlightType == HighlightType.HAND_DISTANCE && handVec.magnitude < highlightDistance)
        {
            Highlight();
        }
        // Else, if look at, check it via the dot procduct
        else if (highlightType == HighlightType.LOOKAT && Vector3.Dot(cameraVec.normalized, cameraPos.forward) > 0.95f)
        {
            Highlight();
        }
        // Else, if point at, check it via the dot procduct
        else if (highlightType == HighlightType.POINTAT && Vector3.Dot(handVec.normalized, gameManager.currentHand.transform.forward) > 0.95f)
        {
            Highlight();
        }
        // Else, dim
        else
        {
            Dim();
        }
    }

    /// <summary>
    /// Highlight the interactive object with a shader.
    /// </summary>
    protected override void Highlight()
    {
        gameObject.GetComponent<Renderer>().material = highlightMaterial;
    }

    /// <summary>
    /// Return to showing the default material.
    /// </summary>
    public override void Dim()
    {
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
    }

    /// <summary>
    /// On interaction.
    /// </summary>
    protected override void Select()
    {
        if (owningEvent != null && !oneClick)
        {
            oneClick = true;
            owningEvent.Clicked();
        }     
    }

    /// <summary>
    /// A method for returning this game object.
    /// </summary>
    /// <returns>This script's owning game object.</returns>
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
