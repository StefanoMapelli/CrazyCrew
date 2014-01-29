using UnityEngine;
using System.Collections;

public class SetDifficulty : MonoBehaviour {

	public string difficulty;
	
	private ServerGameManager serverGameManager;

	void Start() {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
	}

	void OnMouseDown() {
		serverGameManager.setDifficulty(difficulty);
		Camera.main.transform.position = new Vector3(520f,58f,-20f);
	}
}
