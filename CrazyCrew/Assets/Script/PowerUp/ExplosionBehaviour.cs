using UnityEngine;
using System.Collections;

public class ExplosionBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Explosion (this.gameObject));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Explosion(GameObject explosionObject)
	{
		yield return new WaitForSeconds(1);
		Destroy(explosionObject);
	}
}
