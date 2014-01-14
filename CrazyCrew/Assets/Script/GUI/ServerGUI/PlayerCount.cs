using UnityEngine;
using System.Collections;

public class PlayerCount : MonoBehaviour {

	private int number = 0;
	private int max = 3;
	private TextMesh textMesh;
	// Use this for initialization
	void Start () {
		textMesh = (TextMesh)gameObject.GetComponent("TextMesh");
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void incrementNumber() {
		if (number < 3) {
			number++;
			//play della notifica
			gameObject.audio.Play ();
			textMesh.text = number+"/"+max;
		}
	}

	public void decrementNumber() {
		if (number > 0) {
			number--;
			//play della notifica
			gameObject.audio.Play ();
			textMesh.text = number+"/"+max;
		}
	}
}
