﻿using UnityEngine;
using System.Collections;

public class RestartRace : MonoBehaviour {

	public GameObject carMovement;
	public GameObject raceManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		Debug.Log("restart la gara");
		Application.LoadLevel("restartLevel");
		//((RaceManager)raceManager.GetComponent("RaceManager")).RestartRace ();
	}

}
