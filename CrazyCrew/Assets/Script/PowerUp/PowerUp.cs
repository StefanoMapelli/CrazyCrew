using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//rotazione su se stesso
		transform.Rotate(new Vector3(0,2,0));

	
	}

	void OnTriggerEnter(Collider other) {

		Debug.Log("Bonus colpito");
		//quando c'è collisione l'oggetto scompare sotto il terreno
		transform.Translate(new Vector3(0,-10,0));
	}

	void OnTriggerExit(Collider other)
	{
		StartCoroutine(PowerUpResp());
	}

	IEnumerator PowerUpResp()
	{
		//dopo dieci secondi il power up ritorna sul percorso
		yield return new WaitForSeconds(10);
		transform.Translate(new Vector3(0,10,0));
	}
}
