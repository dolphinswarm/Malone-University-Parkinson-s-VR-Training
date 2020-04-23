using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages how the question displays on the Info Board.
/// </summary>
public class QuestionDisplayManager : MonoBehaviour {
    // ======================================================== Properties
    [Header("Info Board Interface")]
    public InfoBoardUI infoBoard;                                   // The info board object being used
	public QuestionEvent myQuestion;                                // The question object
    public GameObject mySubmitButton;                               // The sumbit button
    public GameObject mySelectAll;                                  // The select all button

    [Header("Game Manager")]
    public GameManager gameManager;                                // The current game manager

    [Header("Answers / Answer Tracking")]
    public AnswerManager[] answerObjects = new AnswerManager[4];    // An array of possible answers to the question (defaults to 4)
    Answer[] myAnswers;                                             // An array of the answers to this question
    private int totalAnswersSelected = 0;                           // The total number of selected answers (only applies to multiple choice questions)

    // ======================================================== Methods
    /// <summary>
    /// Intializes the question display manager.
    /// </summary>
    void Awake () {
        // If SubmitButton is not manually added, find it
        if (mySubmitButton == null)
            mySubmitButton = transform.Find("Submit Answer").gameObject;

        // If SelectAll is not manually added, find it
        if (mySelectAll == null)
            mySelectAll = transform.Find("All of the Above").gameObject;

        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager
    }

    /// <summary>
    /// Sets the total number of answers selected. Used by AnswerManager and AllOfTheAbove for multiple-choice questions.
    /// </summary>
    /// <param name="val">Value to which the total selected answers is set</param>
    public void SetTotal(int val) {
        // Set the value, if within the limits
        if (val >= 0 && val <= answerObjects.Length)
            totalAnswersSelected = val;
    }

    /// <summary>
    /// Gets the total number of answers selected.
    /// </summary>
    /// <returns></returns>
    public int GetTotal() { return totalAnswersSelected; }

    /// <summary>
    /// InfoBoardUI uses this to find the submit button before setting active (thus avoiding the "needs assigned" error). DEPRECATED
    /// </summary>
    public void SetSubmitButton()
    {
        if (mySubmitButton == null)
        {
            mySubmitButton = transform.Find("Submit Answer").gameObject;
        }
    }

    /// <summary>
    /// Selects all the answers.
    /// </summary>
    /// <param name="select"></param>
    public void SelectAll(bool select = true) {
        foreach (AnswerManager ans in answerObjects) {
            // This method is more general, and lets us do it either way
            ans.selected = select;
            ans.ToggleSelectionColor();
        }
    }

    /// <summary>
    /// Sets the answers and their individual texts.
    /// </summary>
    /// <param name="newAnswers"></param>
	public void SetAnswers(Answer[] newAnswers) {
        // Set the answer array to be equal to the new answers passed in
        myAnswers = newAnswers;

        // For each answer, set the answer text and toggle features (selected, active, isCorrect, etc.)
		for (int i = 0; i < newAnswers.Length; i++) {
            answerObjects[i].SetAnswerText(newAnswers[i].answerText);
            answerObjects[i].isCorrect = newAnswers[i].isCorrect;
            answerObjects[i].selected = false;
            answerObjects[i].isHighlighted = false;
            answerObjects[i].gameObject.SetActive(true);
        }

        // If the new answers length is greater than the number of answer objects, set the extras to false
        if (newAnswers.Length < answerObjects.Length) {
            for (int extra_i = newAnswers.Length; extra_i < answerObjects.Length; extra_i++) {
                answerObjects[extra_i].gameObject.SetActive(false);
            }
        }
	}

    /// <summary>
    /// Of a new answer is selected, handle it appropriately.
    /// </summary>
    /// <param name="newAns"></param>
    public void NewAnswerSelected (AnswerManager newAns) {
        // If only one answer allowed, then cycle through and delect the other answers
        if (! myQuestion.GetComponent<QuestionEvent>().multipleAnswers) {
            // cycle through answers and deselect all but the new one
            foreach (AnswerManager ans in answerObjects) {
                if (ans != newAns) {
                    ans.selected = false;
                    ans.ToggleSelectionColor();
                }
            }
        }

        // If sumbit on selection, immediately check answers
        if (myQuestion.submitOnSelection)
            CheckAnswers();

        // If multiple choice, check to see if the All of the Above answer should change
        if (myQuestion.GetComponent<QuestionEvent>().multipleAnswers)
            mySelectAll.GetComponent<AllOfTheAbove>().Check(totalAnswersSelected == answerObjects.Length);
    }

    /// <summary>
    /// Checks to see if the currently-selected answer(s) is / are correct.
    /// </summary>
    public void CheckAnswers() {
        // Set the sumbit button (and select all button) to inactive
        mySubmitButton.SetActive(false);
        mySelectAll.SetActive(false);

        if (myQuestion.openEnded) {
          
            // find selected answers
            List <AnswerManager> thingsSelected = new List<AnswerManager>();
            foreach (AnswerManager thisAnswer in answerObjects) {
                // check to see if it's selected
                if (thisAnswer.selected) {
                    // add it to the list if so
                    thingsSelected.Add(thisAnswer);
                }
            }
            // report back to the original question
            myQuestion.ReportAnswer(thingsSelected);
        }
        else {
            // Start by assuming that each answer selection is correct
            bool allIsWell = true;      // ...then work to disprove that notion

            // loop through all answers
            AnswerManager curAnswer;
            for (int i = 0; i < answerObjects.Length; i++) {
                curAnswer = answerObjects[i].GetComponent<AnswerManager>();
                // check to see if it's selected-state (t/f) matches the isCorrect parameter of corresponding answer data
                if (curAnswer.selected != curAnswer.isCorrect)
                {  //...check for mismatch, !=
                   // mismatch found, so all is not well... this question is wrong
                   //  this could be a correct item skipped or a wrong item selected
                    allIsWell = false;
                    // change highlight color to note that this particular selection was wrong
                    answerObjects[i].GetComponent<Text>().color = infoBoard.wrongColor;
                }
                // for correct answers, highlight them to denote correctness
                if (curAnswer.isCorrect)
                {
                    // note that this will over-write the "Wrong" color for skipped items that should have been selected
                    answerObjects[i].GetComponent<Text>().color = infoBoard.rightColor;
                }

            }

            // once looping through all answers is done, report back to QuestionEvent
            myQuestion.SubmitAnswer(allIsWell);
        }

	}
}
