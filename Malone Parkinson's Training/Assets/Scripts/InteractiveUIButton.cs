using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveUIButton : Interactive
{
    // ======================================================== Properties
    [Header("Button Properties")]
    Button button;                      // The button for this object.

    [Header("Management")]
    TitleScreenManager titleScreenManager;

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
            button = gameObject.GetComponent<Button>();

        // Set the events for highlighting, clicking, etc.
        //button.OnSelect(Highlight());
    }

    /// <summary>
    /// Highlight this text.
    /// </summary>
    protected override void Highlight()
    {
        button.Select();
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
        button.onClick.Invoke();
    }


}
