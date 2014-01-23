using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour {

	GameObject car;
	float maxMissileSpeed=60;


	// Use this for initialization
	void Start () {
		car=GameObject.Find("_BogieCarModel");
		GameObject.Find("BonusText").GetComponent<TextMesh>().text="";
		StartCoroutine (DeleteMissile ());
	}

	// Update is called once per frame
	void Update () {

		this.GetComponent<Rigidbody>().AddForce(car.transform.right*maxMissileSpeed, ForceMode.VelocityChange);
	}

	void OnCollisionEnter()
	{
		Explode ();
	}

	void OnTriggerEnter(Collider other)
	{
		Explode ();
	}

	void Explode()
	{
		Debug.Log("explosion");
		UnityEngine.Object explosion=Resources.Load("Prefab/Explosion");
		GameObject explosionObject= GameObject.Instantiate(explosion, gameObject.transform.position,gameObject.transform.rotation) as GameObject;
		StartCoroutine(Explosion (explosionObject));
		Destroy(this.gameObject);
		//this.gameObject.SetActive (false);
	}

	IEnumerator Explosion(GameObject explosionObject)
	{
		yield return new WaitForSeconds(1);
		Destroy(explosionObject);
	}

	IEnumerator DeleteMissile()
	{
		yield return new WaitForSeconds(5);
		Destroy(this.gameObject);
	}
}
