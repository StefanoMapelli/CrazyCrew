using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public Material[] materials = new Material[3];
	public float delay = 3f;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(delay);
		this.GetComponent("MeshRenderer").renderer.material =  materials[1];
		yield return new WaitForSeconds(delay);
		this.GetComponent("MeshRenderer").renderer.material =  materials[2];
		yield return new WaitForSeconds(delay);
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			Application.LoadLevel("server");
		else if (Application.platform == RuntimePlatform.Android)
			Application.LoadLevel ("client");
	}
}

