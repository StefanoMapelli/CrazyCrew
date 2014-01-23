using UnityEngine;
using System.Collections;

public class ReturnButton : MonoBehaviour {
	public AudioSource buttonSound;

	void OnMouseDown() {
		GUIMenusClient.showServerList(false);
		GUIMenusClient.mainMenu(true);
	}

	void OnMouseUp()
	{
		buttonSound.Play ();
	}
}
