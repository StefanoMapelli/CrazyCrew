using UnityEngine;
using System.Collections;

public class SetDifficulty : MonoBehaviour {

	public string difficulty;
	private GameObject server;
	
	private ServerGameManager serverGameManager;

	void Start() {
		server = GameObject.Find("Server");
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
	}

	void OnMouseDown() {
		ServerNetworkManager serverNetworkManager = (ServerNetworkManager) server.GetComponent("ServerNetworkManager");
		serverNetworkManager.StartServer();
		serverGameManager.setDifficulty(difficulty);
		Camera.main.transform.position = new Vector3(520f,58f,-20f);
	}
}
