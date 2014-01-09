using UnityEngine;
using System.Collections;

public class BonusMissile : MonoBehaviour, Bonus 
{

	private GameObject car;
	float missileForwardOffset = 3;
	Vector3 height = new Vector3(0,2,0);

	public BonusMissile()
	{
		car = GameObject.Find("_BogieCarModel");
	}

	public void StartEffect () 
	{
		UnityEngine.Object missile=Resources.Load("Prefab/Missile");
		GameObject missileObject= GameObject.Instantiate(missile, height+(car.transform.position+car.transform.right*missileForwardOffset), car.transform.rotation) as GameObject;
	}
}
