using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	void Awake () {
		float height = Camera.main.orthographicSize * 2f;
		float width = (height * Screen.width / Screen.height);

		transform.localScale = new Vector3(width,height);
	}
}
