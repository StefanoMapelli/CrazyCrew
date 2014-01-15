using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CheckPointManager : MonoBehaviour {

	private RaceManager raceManager;
	private Dictionary<string,TimeSpan> timeList=new Dictionary<string,TimeSpan>();

	// Use this for initialization
	void Start () {

		raceManager=((RaceManager)GameObject.Find("RaceManager").GetComponent ("RaceManager"));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag=="CarCollider")
		{
			if(timeList.ContainsKey(other.gameObject.name))
			{
				timeList.Remove(other.gameObject.name);
			}

			timeList.Add(other.gameObject.name,raceManager.getTime());

			if(other.name=="Player")
			{
				raceManager.ShowRanking(timeList);
			}
		}
	}
}
