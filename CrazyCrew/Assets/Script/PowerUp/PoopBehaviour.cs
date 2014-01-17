using UnityEngine;
using System.Collections;

public class PoopBehaviour : MonoBehaviour {

	public AudioSource collisionPoopSound;

	// Use this for initialization
	void Start () {
		GameObject.Find("BonusText").GetComponent<TextMesh>().text="";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision c)
	{
		if(!(c.collider.tag=="Terrain"))
		{
			collisionPoopSound.Play();
			Destroy(this.gameObject);
		}
	}
}
