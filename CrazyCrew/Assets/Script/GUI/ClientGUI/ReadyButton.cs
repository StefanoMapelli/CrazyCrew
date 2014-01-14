using UnityEngine;
using System.Collections;

public class ReadyButton : MonoBehaviour {

	private ClientGameManager clientGameManager;

	// Use this for initialization
	void Start () {
		clientGameManager = (ClientGameManager) GameObject.Find ("Client").GetComponent ("ClientGameManager");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,Screen.height/2, screenPos.z));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		//if(((TextMesh)gameObject.GetComponent("TextMesh")).text.EndsWith("?"))

			((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;

		clientGameManager.setReady(Network.player);
		//GUIMenusClient.readyButton(false);
	}

	void OnMouseUp()
	{
		//if(((TextMesh)gameObject.GetComponent("TextMesh")).text.EndsWith("?"))

			((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		gameObject.audio.Play();
	}
}
