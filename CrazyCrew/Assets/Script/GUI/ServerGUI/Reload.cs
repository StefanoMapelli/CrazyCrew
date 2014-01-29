using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {
	private ServerGameManager serverGameManager;

	private Preview preview;
	private TextMesh highscore;

	void Start() {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
		preview = (Preview) GameObject.Find ("Preview").GetComponent("Preview");
		highscore = (TextMesh) GameObject.Find ("Highscore").GetComponent("TextMesh");
	}

	void OnMouseDown() {
		serverGameManager.disconnectPlayers();
		MasterServer.UnregisterHost();
		preview.setMaterial(0);
		highscore.text = "";
	}
}
