﻿using UnityEngine;
using System.Collections;

public class BonusSpeed : MonoBehaviour, Bonus 
{
	private BogieCarMovement bogieCarMovement; 

	public BonusSpeed()
	{
		bogieCarMovement = (BogieCarMovement) GameObject.Find("BogieCarModel").GetComponent("BogieCarMovement");
	}

	//effetto: chiama la courutine di BogieCarMovement che da una forza alla macchina per 5 secondi
	public void StartEffect ()
	{
		bogieCarMovement.bonusSpeed();
	}
}
