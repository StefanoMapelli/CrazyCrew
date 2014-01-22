﻿using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {
	private ServerGameManager serverGameManager;
	public AudioSource buttonSound;

	void Start() {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
	}

	void OnMouseDown() {
		Camera.main.transform.position = new Vector3(0f,0f,-20f);
		serverGameManager.disconnectPlayers();
		MasterServer.UnregisterHost();
	}

	void OnMouseUp()
	{
		buttonSound.Play();
	}
}
