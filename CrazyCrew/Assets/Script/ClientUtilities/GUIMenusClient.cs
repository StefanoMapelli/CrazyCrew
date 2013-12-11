using UnityEngine;
using System.Collections;

public static class GUIMenusClient {

	public static void refreshList(bool enabled) {
		GameObject refreshList = GameObject.Find("RefreshListText");

		((MeshRenderer) refreshList.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) refreshList.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void quitButton(bool enabled) {
		GameObject quitButton = GameObject.Find("Quit");

		((MeshRenderer) quitButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) quitButton.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void connectionError(bool enabled) {
		GameObject connectionError = GameObject.Find("ConnectionErrorText");

		((MeshRenderer) connectionError.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void readyButton(bool enabled) {
		GameObject readyButton = GameObject.Find("ReadyText");

		((MeshRenderer) readyButton.GetComponent("MeshRenderer")).enabled = enabled;
		((BoxCollider) readyButton.GetComponent("BoxCollider")).enabled = enabled;
	}

	public static void readyButton(string msg) {
		TextMesh readyButtonMesh = (TextMesh) GameObject.Find("ReadyText").GetComponent("TextMesh");
		readyButtonMesh.text = msg;
	}
}
