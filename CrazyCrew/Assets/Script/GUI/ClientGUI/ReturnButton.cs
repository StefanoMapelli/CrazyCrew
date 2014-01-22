using UnityEngine;
using System.Collections;

public class ReturnButton : MonoBehaviour {

	void OnMouseDown() {
		GUIMenusClient.showServerList(false);
		GUIMenusClient.mainMenu(true);
	}
}
