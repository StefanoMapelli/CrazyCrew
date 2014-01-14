﻿using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

	public AudioSource buttonSound;

	// Use this for initialization
	void Start () {
		/*Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		Application.Quit();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		buttonSound.Play();
	}
}
