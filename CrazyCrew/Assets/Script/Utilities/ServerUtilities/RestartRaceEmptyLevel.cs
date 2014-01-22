using UnityEngine;
using System.Collections;

public class RestartRaceEmptyLevel : MonoBehaviour {

	private ServerGameManager serverGameManager;

	// Use this for initialization
	void Start () {
		serverGameManager = (ServerGameManager) GameObject.Find ("Server").GetComponent("ServerGameManager");

		if (serverGameManager.getLevel() == 1)
			Application.LoadLevel("bogieCar");
		else if (serverGameManager.getLevel() == 2)
			Application.LoadLevel ("bogieCar2");
	}

	public void OnLevelWasLoaded(int level) {
		if (level == 1 || level == 2) {
			((RaceManager)GameObject.Find("RaceManager").GetComponent("RaceManager")).RestartRace ();
		}
	}
}
