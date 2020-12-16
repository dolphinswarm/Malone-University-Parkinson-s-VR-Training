using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LIWSScript : MonoBehaviour
{
    // ======================================================== Variables
    [Header("Controllers")]
    public TMP_Text text;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start()
    {
        text.text = "OFF";
    }

    /// <summary>
    /// Changes the text of the child text object
    /// </summary>
    /// <param name="newText"></param>
    void ChangeText(string newText)
    {
        text.text = newText;
    }
}
