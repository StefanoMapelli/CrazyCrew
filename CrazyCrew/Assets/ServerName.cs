using UnityEngine;
using System.Collections;

public class ServerName : MonoBehaviour {

	public TextMesh serverText;

	// Use this for initialization
	void Start () {
		serverText.text+="\n"+System.Environment.UserName+" + start time";	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
