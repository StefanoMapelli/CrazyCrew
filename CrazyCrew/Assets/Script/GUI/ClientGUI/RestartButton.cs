using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

	private ClientGameManager clientGameManager;
	
	// Use this for initialization
	void Start () {
		clientGameManager = (ClientGameManager) GameObject.Find ("Client").GetComponent("ClientGameManager");
	}

	void OnMouseDown() {
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		clientGameManager.restart();
		clientGameManager.setPause(false);
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
	}
}
