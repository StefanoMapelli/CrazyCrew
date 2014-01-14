using UnityEngine;
using System.Collections;

public class RefreshList : MonoBehaviour {
	
	private ClientNetworkManager clientNetworkManager;
	public AudioSource buttonSound;

	// Use this for initialization
	void Start () {
		clientNetworkManager = (ClientNetworkManager)GameObject.Find("Client").GetComponent("ClientNetworkManager");
	}
		/*
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));*/

	void OnMouseDown() {
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		clientNetworkManager.RefreshHostList();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		buttonSound.audio.Play();
	}
}
