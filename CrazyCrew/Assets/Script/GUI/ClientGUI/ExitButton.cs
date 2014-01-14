using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {

	private ClientGameManager clientGameManager;

	// Use this for initialization
	void Start () {
		clientGameManager = (ClientGameManager) GameObject.Find ("Client").GetComponent("ClientGameManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		clientGameManager.exit();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		gameObject.audio.Play();
	}
}
