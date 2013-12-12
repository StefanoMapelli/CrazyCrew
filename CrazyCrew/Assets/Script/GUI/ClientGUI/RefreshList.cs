﻿using UnityEngine;
using System.Collections;

public class RefreshList : MonoBehaviour {

	private ClientNetworkManager clientNetworkManager;

	// Use this for initialization
	void Start () {
		clientNetworkManager = (ClientNetworkManager)GameObject.Find("Client").GetComponent("ClientNetworkManager");

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/2,(Screen.height/3)*2, screenPos.z));
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		clientNetworkManager.RefreshHostList();
	}
}
