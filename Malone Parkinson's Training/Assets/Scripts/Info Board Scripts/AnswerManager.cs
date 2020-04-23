using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// An answer on the question board.
/// </summary>
public class AnswerManager : InteractiveText {
    // ======================================================== Variables
    [Header("Answer Info")]
    public string answerText;
	public bool selected = false;
    public bool isCorrect = false;

    // ======================================================== Methods
    /// <summary>
    /// Toggle the selection color, depending on if the answer is currently selected.
    /// </summary>
    public void ToggleSelectionColor() {
        if (gameObject.activeInHierarchy) {
            if (selected) {
                myText.color = infoBoard.selectedColor;
                Highlight();
            }// + infoBoard.highlightTint; }
            else {
                myText.color = infoBoard.normalColor;
                Highlight();
            }// + infoBoard.highlightTint; }
        }
    }

    /// <summary>
    /// Select this answer.
    /// </summary>
    protected override void Select() {
        // Change the selected status of this GameObject
        selected = !selected;

        // Change the selection color
        if (!myQuestion.myQuestion.GetComponent<QuestionEvent>().hasBeenSubmitted) ToggleSelectionColor();

        // Update the number of selected items, if multiple choice
        if (myQuestion.myQuestion.GetComponent<QuestionEvent>().multipleAnswers)
        {
            if (selected)
                myQuestion.SetTotal(myQuestion.GetTotal() + 1);
            else
                myQuestion.SetTotal(myQuestion.GetTotal() - 1);
        }

        // need to tell the QuestionDisplayManager that I was selected
        transform.parent.GetComponent<QuestionDisplayManager>().NewAnswerSelected(this);
    }

    /// <summary>
    /// Called externally to set the text displayed on the answer.
    /// </summary>
    /// <param name="newAnswerText"></param>
    public void SetAnswerText (string newAnswerText) {
        // Initalize this object
        Initialize();

        // Set the answer text to the passed-in answer text
        answerText = newAnswerText;
        myText.text = answerText;
    }

    /// <summary>
    /// Called when clicked off.
    /// </summary>
    /// <returns></returns>
    IEnumerator ClickedOff()
    {
        yield return new WaitForSeconds(1);
        //hasBeenClicked = false;
        StopCoroutine(ClickedOff());
    }

}
