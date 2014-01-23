using UnityEngine;
using System.Collections;

public class SetLevel : MonoBehaviour {

	private GameObject server;
	private Preview preview;

	public AudioSource buttonSound;

	public int level;


	// Use this for initialization
	void Start () {
		server = GameObject.Find("Server");
		preview = (Preview) GameObject.Find ("Preview").GetComponent("Preview");
	}
	
	void OnMouseDown() {
		ServerGameManager serverGameManager = (ServerGameManager) server.GetComponent("ServerGameManager");

		serverGameManager.setLevel(level);
		preview.setMaterial(level);
	}

	void OnMouseUp()
	{
		buttonSound.Play();
	}
}
