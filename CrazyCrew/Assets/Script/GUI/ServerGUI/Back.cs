using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		Camera.main.transform.position = new Vector3(-420f,0f,-20f);
	}
}
