using UnityEngine;
using System.Collections;

public class BackFromLevels : MonoBehaviour {

	public AudioSource buttonSound;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		Camera.main.transform.position = new Vector3(0f,0f,-20f);
	}

	// Update is called once per frame
	void Update () {
	
	}

	
	void OnMouseUp()
	{
		buttonSound.Play();
	}
}
