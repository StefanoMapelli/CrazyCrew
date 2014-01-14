using UnityEngine;
using System.Collections;

public class ExitToInitialMenu : MonoBehaviour {

	private ServerGameManager serverGameManager;

	// Use this for initialization
	void Start () {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		serverGameManager.exitGame();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		gameObject.audio.Play ();
	}
}