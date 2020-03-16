using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script for any interactive text
/// </summary>
public class InteractiveText : Interactive {
    // ======================================================== Properties
    [Header("Text Properties")]
    protected QuestionDisplayManager myQuestion;
    protected Text myText;

    // ======================================================== Methods
    /// <summary>
    /// Initialize this interactive text.
    /// </summary>
    protected override void Initialize() {
        // Get the question display manager, info board UI, text, and color
        myQuestion = transform.parent.GetComponent<QuestionDisplayManager>();
        infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one
        myText = GetComponent<Text>();
        myText.color = infoBoard.normalColor;

        // Call base intialize
        base.Initialize();
    }

    protected override void Highlight() {
        myColor = GetComponent<Text>().color;
        GetComponent<Text>().color = new Vector4(
                myColor.r + infoBoard.highlightPct,
                myColor.g + infoBoard.highlightPct,
                myColor.b + infoBoard.highlightPct,
                myColor.a);
    }

    public override void Dim() {
        myColor = GetComponent<Text>().color;
        GetComponent<Text>().color = new Vector4(
                myColor.r - infoBoard.highlightPct,
                myColor.g - infoBoard.highlightPct,
                myColor.b - infoBoard.highlightPct,
                myColor.a);
    }


}
