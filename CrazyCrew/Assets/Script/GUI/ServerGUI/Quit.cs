using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {

	public AudioSource buttonSound;

	// Use this for initialization
	void OnMouseDown()
	{
		Application.Quit();
	}

	void OnMouseUp()
	{
		buttonSound.Play ();
	}
}
