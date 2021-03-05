using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveUIButton : Interactive
{
    // ======================================================== Properties
    [Header("Button Properties")]
    public Button button;                      // The button for this object.

    [Header("Audio")]
    public AudioClip selectClip;
    public AudioClip highlightClip;

    [Header("Management")]
    public TitleScreenManager titleScreenManager;
    bool pressed = false;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        // Add the title screen manager, if not set
        if (titleScreenManager == null)
            titleScreenManager = FindObjectOfType<TitleScreenManager>();

        // Add a button, if not already set
        if (button == null)
            button = GetComponent<Button>();

        // Set the events for highlighting, clicking, etc.
        //button.OnSelect(Highlight());
        base.Start();
    }

    /// <summary>
    /// Highlight this text.
    /// </summary>
    protected override void Highlight()
    {
        if (!titleScreenManager.hasButtonBeenPressed)
        {
            if (highlightClip != null) AudioSource.PlayClipAtPoint(highlightClip, transform.position);
            button.Select();
        }
    }

    /// <summary>
    /// Dim this text.
    /// </summary>
    public override void Dim()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Toggles this button.
    /// </summary>
    protected override void Select()
    {
        if (!titleScreenManager.hasButtonBeenPressed && !pressed && !OVRInput.GetDown(OVRInput.RawButton.A))
        {
            button.onClick.Invoke();
            titleScreenManager.StopMusic();
            if (selectClip != null) AudioSource.PlayClipAtPoint(selectClip, transform.position);
            pressed = true;
        }
    }


}
