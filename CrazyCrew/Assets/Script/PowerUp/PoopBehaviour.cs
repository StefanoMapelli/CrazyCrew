using UnityEngine;
using System.Collections;

public class PoopBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if(!(c.tag=="Terrain"))
		{
			Destroy(this.gameObject);
		}
	}
}
