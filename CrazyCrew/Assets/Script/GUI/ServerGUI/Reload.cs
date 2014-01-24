using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {
	private ServerGameManager serverGameManager;

	private Preview preview;

	void Start() {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
		preview = (Preview) GameObject.Find ("Preview").GetComponent("Preview");
	}

	void OnMouseDown() {
		serverGameManager.disconnectPlayers();
		MasterServer.UnregisterHost();
		preview.setMaterial(0);
	}
}
