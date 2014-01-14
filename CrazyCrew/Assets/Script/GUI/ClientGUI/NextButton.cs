using UnityEngine;
using System.Collections;

public class NextButton : MonoBehaviour {

	private ServerList serverList;

	// Use this for initialization
	void Start () {
		serverList = (ServerList) GameObject.Find ("ServerList").GetComponent("ServerList");
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		serverList.incrementIdex();
	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
		gameObject.audio.Play();
	}
}
