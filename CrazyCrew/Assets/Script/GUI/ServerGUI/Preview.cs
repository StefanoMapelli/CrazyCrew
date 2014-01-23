using UnityEngine;
using System.Collections;

public class Preview : MonoBehaviour {

	public Material[] materials = new Material[3];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Camera.main.transform.Translate(new Vector3(940f,58f,0f));
	}

	public void setMaterial(int m) {
		this.GetComponent("MeshRenderer").renderer.material = materials[m];
	}
}
