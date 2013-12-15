using UnityEngine;
using System.Collections;

public class ExitToInitialMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		GameObject.Destroy(GameObject.Find("Server"));
		Application.LoadLevel("server");
	}
}