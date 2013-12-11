using UnityEngine;
using System.Collections;

public static class GUIMenusClient {

	public static void refreshList(bool enabled) {
		MeshRenderer refreshListRenderer = (MeshRenderer) GameObject.Find("RefreshListText").GetComponent("MeshRenderer");
		refreshListRenderer.enabled = enabled;
	}

	public static void quitButton(bool enabled) {
		MeshRenderer quitButtonRenderer = (MeshRenderer) GameObject.Find("Quit").GetComponent("MeshRenderer");
		quitButtonRenderer.enabled = enabled;
	}

	public static void connectionError(bool enabled) {
		MeshRenderer connectionErrorRenderer = (MeshRenderer) GameObject.Find("ConnectionErrorText").GetComponent("MeshRenderer");
		connectionErrorRenderer.enabled = enabled;
	}

	public static void readyButton(bool enabled) {
		MeshRenderer readyButtonRenderer = (MeshRenderer) GameObject.Find("ReadyText").GetComponent("MeshRenderer");
		readyButtonRenderer.enabled = enabled;
	}

	public static void readyButton(string msg) {
		TextMesh readyButtonMesh = (TextMesh) GameObject.Find("ReadyText").GetComponent("TextMesh");
		readyButtonMesh.text = msg;
	}
}
