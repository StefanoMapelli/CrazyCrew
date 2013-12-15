using UnityEngine;
using System.Collections;

public class ExitToMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		Debug.Log("Ho schiacciato");
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.black;
		GameObject.Destroy(GameObject.Find("Client"));
		Network.Disconnect();
		Application.LoadLevel("client");

	}

	void OnMouseUp()
	{
		((TextMesh)gameObject.GetComponent("TextMesh")).color = Color.white;
	}
}
