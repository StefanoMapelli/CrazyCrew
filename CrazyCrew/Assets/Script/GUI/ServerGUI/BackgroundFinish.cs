using UnityEngine;
using System.Collections;

public class BackgroundFinish : MonoBehaviour {

	void Awake () {
		Camera camera = (Camera) GameObject.Find ("Camera1").GetComponent ("Camera");

		float height = camera.orthographicSize * 2f;
		float width = (height * Screen.width / Screen.height);
		
		transform.localScale = new Vector3(width,height);
	}
}
