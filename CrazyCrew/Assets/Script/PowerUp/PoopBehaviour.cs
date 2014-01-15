using UnityEngine;
using System.Collections;

public class PoopBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision c)
	{
		if(!(c.collider.tag=="Terrain"))
		{
			Destroy(this.gameObject);
		}
	}
}
