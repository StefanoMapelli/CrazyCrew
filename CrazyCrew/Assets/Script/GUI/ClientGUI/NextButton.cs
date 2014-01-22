using UnityEngine;
using System.Collections;

public class NextButton : MonoBehaviour {

	private ServerList serverList;
	public AudioSource shiftSound;

	// Use this for initialization
	void Start () {
		serverList = (ServerList) GameObject.Find ("ServerList").GetComponent("ServerList");
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		serverList.incrementIdex();
	}

	void OnMouseUp()
	{
		shiftSound.Play();
	}
}
