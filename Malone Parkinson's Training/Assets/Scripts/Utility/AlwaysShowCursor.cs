using UnityEngine;
using System.Collections;

/// <summary>
/// Script for always showing the cursor.
/// </summary>
public class AlwaysShowCursor : MonoBehaviour {
	/// <summary>
    /// On frame update, make the cursor always shown.
    /// </summary>
	void Update () {
		// If cursor is not visible, make it visible
		if(Cursor.visible == false)
			Cursor.visible = true;
	}
}
