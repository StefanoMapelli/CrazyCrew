using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour {

	GameObject car;
	float maxMissileSpeed=60;


	// Use this for initialization
	void Start () {
		car=GameObject.Find("_BogieCarModel");
	}
	
	// Update is called once per frame
	void Update () {

		this.GetComponent<Rigidbody>().AddForce(car.transform.right*maxMissileSpeed, ForceMode.VelocityChange);
	}

	void OnCollisionEnter()
	{
		Destroy(this.gameObject);
	}
}
