using UnityEngine;
using System.Collections;

public class SetLevel : MonoBehaviour {

	private GameObject server;

	public int level;

	// Use this for initialization
	void Start () {
		server = GameObject.Find("Server");
	}
	
	void OnMouseDown() {
		ServerGameManager serverGameManager = (ServerGameManager) server.GetComponent("ServerGameManager");

		serverGameManager.setLevel(level);
		Camera.main.transform.Translate(new Vector3(940f,58f,0f));
	}
}
