using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ReportCardManager : MonoBehaviour
{
    // ======================================================== Variables
    // Declaration of score variables accessible by external objects. Used for "Scoring" a 
    // given report card.
    public int currentScore = 0;
    public int totalScore = 0;

	// The filename of the file to write the report card to.
	public string enteredFileName = "REPORTCARD";

    // The filename of the file to write to
    private string fileName = "";

    // Should we write to the report card?
    public bool shouldWriteToReportCard = true;

    // ======================================================== Methods
    /// <summary>
    /// On game start...
    /// </summary>
    void Start ()
    {
		// Formats the filename using the entered file name, and formats it so that all 
		// report cards are put in the ReportCards directory, and are formatted to match
		// the format "FileName MM-DD-YYYY HH.MM.SS.txt".
		DateTime today = System.DateTime.Now;
		fileName += "ReportCards/";
		fileName += enteredFileName;
		fileName += " " + today.Month.ToString () + "-" + today.Day.ToString() + "-" + today.Year.ToString() + " " + today.Hour.ToString () + "." + today.Minute.ToString () + "." + today.Second.ToString ();
		fileName += ".txt";
		Debug.Log (fileName);

		// If the file already exists (In case of a weird error), then the previous file is
		// deleted and overwritten by the new file.
		if(File.Exists(fileName))
        {
			Debug.Log("File " + '"' + fileName + '"' + " already exists.");
			System.IO.File.Delete(fileName);
		}

        // Timestamps the first line of the file with the current date and time.
        writeLine("=========================");
        writeLine("Malone University - Parkinson's VR Training");
        writeLine("TEST OCCURED ON " + DateTime.Now.ToString());
        writeLine("=========================");
        writeLine("");

    }


    /// <summary>
    /// Public method which can be called by external objects for the purpose of writing to the reportcard. 
    /// </summary>
    /// <param name="str">The line(s) to write to.</param>
    public void writeLine(string str)
    {
		str += Environment.NewLine;
		System.IO.File.AppendAllText(fileName, str);
	}

    /// <summary>
    /// Returns the report card's name.
    /// </summary>
    /// <returns>The name of the report card in string format.</returns>
    public string getFileName()
    {
        return fileName.ToString();
    }
}
