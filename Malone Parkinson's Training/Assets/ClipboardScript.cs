using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// The script for handling the clipboard.
/// </summary>
public class ClipboardScript : MonoBehaviour
{
    // ======================================================== Variables
    [Header("Text")]
    public TMP_Text titleText;          // The text for the title.
    public TMP_Text colOneText;         // The text for the first column.
    public TMP_Text colTwoText;         // The text for the second column.

    // ======================================================== Methods
    /// <summary>
    /// Initilize stuff on game start.
    /// </summary>
    void Start()
    {
        // Sets the title text, if not set
        if (titleText == null)
            titleText = GameObject.Find("Title Text").GetComponent<TMP_Text>();

        // Sets column one, if not set
        if (colOneText == null)
            titleText = GameObject.Find("Text (col. 1)").GetComponent<TMP_Text>();

        // Sets column two, if not set
        if (colTwoText == null)
            titleText = GameObject.Find("Text (col. 2)").GetComponent<TMP_Text>();

    }

    ///// <summary>
    ///// Sets the text in column one.
    ///// </summary>
    //public void SetColOneText(string text)
    //{

    //}

    ///// <summary>
    ///// Sets the text in column twice.
    ///// </summary>
    //public void SetColTwoText(string text)
    //{

    //}
    
    ///// <summary>
    ///// Set the title text.
    ///// </summary>
    //public void SetTitleText(string text)
    //{

    //}

}
