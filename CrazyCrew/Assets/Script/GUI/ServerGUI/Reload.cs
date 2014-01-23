using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {
	private ServerGameManager serverGameManager;
	public AudioSource buttonSound;

	private Preview preview;

	void Start() {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
		preview = (Preview) GameObject.Find ("Preview").GetComponent("Preview");
	}

	void OnMouseDown() {
		Camera.main.transform.position = new Vector3(0f,0f,-20f);
		serverGameManager.disconnectPlayers();
		MasterServer.UnregisterHost();
		preview.setMaterial(0);
	}

	void OnMouseUp()
	{
		buttonSound.Play();
	}
}
