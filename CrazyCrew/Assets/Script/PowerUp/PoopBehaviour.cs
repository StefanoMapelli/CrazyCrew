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
		Debug.Log("trigger");
		if(!(c.tag=="Terrain"))
		{
			Debug.Log("destroy");
			Destroy(this.gameObject);
		}
	}
}
