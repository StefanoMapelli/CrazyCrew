using UnityEngine;
using System.Collections;

public class SetLevel : MonoBehaviour {

	private GameObject server;
	private Preview preview;
	private TextMesh highscore;

	public AudioSource buttonSound;

	public int level;


	// Use this for initialization
	void Start () {
		server = GameObject.Find("Server");
		preview = (Preview) GameObject.Find ("Preview").GetComponent("Preview");
		highscore = (TextMesh) GameObject.Find("Highscore").GetComponent("TextMesh");
	}
	
	void OnMouseDown() {
		ServerGameManager serverGameManager = (ServerGameManager) server.GetComponent("ServerGameManager");

		serverGameManager.setLevel(level);
		preview.setMaterial(level);

		if (level == 1) {
			if (PlayerPrefs.HasKey ("highmin1") && PlayerPrefs.HasKey ("highsec1") && PlayerPrefs.HasKey ("highmsec1")) 
				highscore.text = string.Format("{0:D2}:{1:D2}:{2:D3}",PlayerPrefs.GetInt ("highmin1"),PlayerPrefs.GetInt ("highsec1"),PlayerPrefs.GetInt ("highmsec1"));
			else
				highscore.text = "No highscore";
		}
		else if (level == 2) {
			if (PlayerPrefs.HasKey ("highmin2") && PlayerPrefs.HasKey ("highsec2") && PlayerPrefs.HasKey ("highmsec2")) 
				highscore.text = string.Format("{0:D2}:{1:D2}:{2:D3}",PlayerPrefs.GetInt ("highmin2"),PlayerPrefs.GetInt ("highsec2"),PlayerPrefs.GetInt ("highmsec2"));
			else {
				Debug.Log (PlayerPrefs.HasKey("highmin2")+" "+PlayerPrefs.HasKey("highsec2")+" "+PlayerPrefs.HasKey("highmsec2"));
				highscore.text = "No highscore";
			}
		}
	}

	void OnMouseUp()
	{
		buttonSound.Play();
	}
}
