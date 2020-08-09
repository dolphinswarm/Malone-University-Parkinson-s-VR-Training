using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The All of the Above option on a multiple-choice question.
/// </summary>
public class AllOfTheAbove : InteractiveText {
    // ======================================================== Variables
    [Header("Display Text")]
    public string selectAll_txt = "All of the Above";
    public string deselectAll_txt = "Deselect All";

    [Header("Properties")]
    private bool allSelected = false;

    // ======================================================== Methods
    /// <summary>
    /// If selected, toggle / untoggle all answers.
    /// </summary>
    protected override void Select ()
    {
        // Toggle if all are selected
        allSelected = !allSelected;

        // Select / deselect all answers
        myQuestion.SelectAll(allSelected);

        // If all are selected, change the text to the deselect text and update the total number of active answer objects
        if (allSelected)
        {
            myText.text = deselectAll_txt;
            myQuestion.SetTotal(myQuestion.answerObjects.Length);
        }

        // Else, change the text to the select text and clear the number of active answer objects
        else
        {
            myText.text = selectAll_txt;
            myQuestion.SetTotal(0);
        }     
    }

    /// <summary>
    ///  Checks if all answers are selected (from Question Display Manager).
    /// </summary>
    /// <param name="areAllSelected">True if all are selected, false if not.</param>
    public void Check(bool areAllSelected)
    {
        // Change if all items are selected
        allSelected = areAllSelected;

        // Check to make sure this is initialized
        if (myText == null) Initialize();

        // Change the text
        if (areAllSelected)
            myText.text = deselectAll_txt;
        else
            myText.text = selectAll_txt;
    }
}
