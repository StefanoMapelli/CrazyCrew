﻿using UnityEngine;
using System.Collections;

public class BonusPoop : MonoBehaviour, Bonus 
{
	private GameObject car;
	private float poopDisplacement = 5f;
	
	public BonusPoop()
	{
		car = GameObject.Find("_BogieCarModel");
	}
	
	public void StartEffect () 
	{
		UnityEngine.Object poop = Resources.Load("Prefab/Poop");
		((BogieCarMovement) car.GetComponent("BogieCarMovement")).usePoop();
		((BogieCarMovement) car.GetComponent("BogieCarMovement")).resetPlank();
		GameObject poopObject= GameObject.Instantiate(poop,car.transform.position+(-car.transform.right)*poopDisplacement, car.transform.rotation) as GameObject;
		//poopObject.rigidbody.AddForce(car.transform.right*300f,ForceMode.Impulse);
	}
}
