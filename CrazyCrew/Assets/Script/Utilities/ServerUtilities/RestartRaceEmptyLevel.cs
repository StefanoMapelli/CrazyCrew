﻿using UnityEngine;
using System.Collections;

public class RestartRaceEmptyLevel : MonoBehaviour {



	// Use this for initialization
	void Start () {

		Application.LoadLevel("bogieCar");
	}

	public void OnLevelWasLoaded(int level) {
		if (level == 1) {
			((RaceManager)GameObject.Find("RaceManager").GetComponent("RaceManager")).RestartRace ();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
