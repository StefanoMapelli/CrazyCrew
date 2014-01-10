using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

	private ClientGameManager clientGameManager;
	private PowerUpController powerUpController;
	
	// Use this for initialization
	void Start () {
		clientGameManager = (ClientGameManager) GameObject.Find ("Client").GetComponent("ClientGameManager");
		powerUpController = (PowerUpController) GameObject.Find ("PowerUpButton").GetComponent("PowerUpController");
	}

	void OnMouseDown() {
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		clientGameManager.restart();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
	}
}
