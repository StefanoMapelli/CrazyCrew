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

		Vector3 screenPos = Camera.main.WorldToScreenPoint(connectionError.transform.position);
		connectionError.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/10, screenPos.z));

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

	public static void pauseMenuPositioning(bool enabled)
	{
		GameObject resumeGameButton = GameObject.Find("ResumeGameButton");
		Vector3 screenPosResume = Camera.main.WorldToScreenPoint(resumeGameButton.transform.position);

		GameObject exitButton = GameObject.Find("ExitToMenuButton");
		Vector3 screenPosExit = Camera.main.WorldToScreenPoint(exitButton.transform.position);

		if(enabled)
		{
			resumeGameButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPosResume.z));
			exitButton.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPosExit.z));
		}
		else
		{
			resumeGameButton.transform.position = new Vector3(1000,1,-2);
			exitButton.transform.position = new Vector3(1000,1,-2);
		}
	}
}
