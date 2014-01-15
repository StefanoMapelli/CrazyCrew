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
		UnityEngine.Object explosion=Resources.Load("Prefab/Explosion");
		GameObject explosionObject= GameObject.Instantiate(explosion, gameObject.transform.position,gameObject.transform.rotation) as GameObject;
		Destroy(this.gameObject);
		StartCoroutine(Explosion (explosionObject));
	}

	IEnumerator Explosion(GameObject explosionObject)
	{
		yield return new WaitForSeconds(1);
		Destroy(explosionObject);
	}
}
