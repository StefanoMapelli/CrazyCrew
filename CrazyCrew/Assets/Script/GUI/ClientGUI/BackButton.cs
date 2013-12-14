using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	private ServerList serverList;
	
	// Use this for initialization
	void Start () {
		serverList = (ServerList) GameObject.Find ("ServerList").GetComponent("ServerList");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown() {
		serverList.decrementIndex();
	}
}
