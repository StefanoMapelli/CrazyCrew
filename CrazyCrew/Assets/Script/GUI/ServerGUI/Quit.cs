using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {

	public AudioSource buttonSound;

	// Use this for initialization
	void OnMouseDown()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;

		Application.Quit();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		buttonSound.Play ();
	}
}
