using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

	private ClientGameManager clientGameManager;

	// Use this for initialization
	void Start () {

		GameObject client=GameObject.Find("Client");
		clientGameManager=(ClientGameManager)client.GetComponent("ClientGameManager");
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown()
	{
		clientGameManager.pauseOn();
	}
}
