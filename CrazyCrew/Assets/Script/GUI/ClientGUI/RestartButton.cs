using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

	private ClientGameManager clientGameManager;
	
	// Use this for initialization
	void Start () {
		clientGameManager = (ClientGameManager) GameObject.Find ("Client").GetComponent("ClientGameManager");
	}

	void OnMouseDown() {
		clientGameManager.restart();
	}
}
