using UnityEngine;
using System.Collections;

public class CristalRotation : MonoBehaviour {

	public Transform axis;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(axis.right,2);
	
	}
}
