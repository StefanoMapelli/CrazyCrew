using UnityEngine;
using System.Collections;

public class RestartRace : MonoBehaviour {

	public GameObject carMovement;
	public GameObject raceManager;
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
		serverGameManager.restartGame();
		//((RaceManager)raceManager.GetComponent("RaceManager")).RestartRace ();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
	}

}
