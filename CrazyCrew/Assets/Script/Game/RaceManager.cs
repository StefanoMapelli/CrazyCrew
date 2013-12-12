using UnityEngine;
using System.Collections;
using System;

public class RaceManager : MonoBehaviour {

	//evento fine gara
	public delegate void RaceFinishEventHandler(object sender, EventArgs e);

	public event EventHandler RaceFinish;

	protected virtual void OnRaceFinish(EventArgs e)
	{
		RaceFinish(this,e);
	}



	private TextMesh infoText;
	//Pause
	private bool pause = true;

	//timer
	private DateTime startTime;
	TimeSpan timer = new TimeSpan(0,0,0,0,0);

	private TextMesh timerText;

	//Menu finale
	public GameObject finishMenu;
	public GameObject finalTime;
	private bool isFinish=false;

	// Use this for initialization
	void Start () {
		//CountDown
		StartCoroutine (StartGame ());
	}

	public void RestartRace () {
		//CountDown
		StartCoroutine (StartGame ());
	}

	/// <summary>
	/// Set up timer, manage initial countDown.
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartGame()
	{
		infoText =  (TextMesh) (GameObject.Find("InfoText").GetComponent("TextMesh"));
		infoText.text="";

		infoText.text = "3";
		yield return new WaitForSeconds(1);
		infoText.text = "2";
		yield return new WaitForSeconds(1);
		infoText.text = "1";
		yield return new WaitForSeconds(1);
		infoText.text = "Go!!";

		//Appena esce la scritta Go si può partire e parte il timer
		timerText =  (TextMesh) (GameObject.Find("TimerText").GetComponent("TextMesh"));
		timerText.text = "00:00:000";
		startTime = DateTime.Now;

		SetPause (false);

		yield return new WaitForSeconds(1);
		infoText.text = "";
	}

	public void SetPause(bool pause)
	{
		this.pause = pause;
	}

	/// <summary>
	/// Il veicolo è giunto alla fine del percorso.
	/// </summary>
	public void FinishLine()
	{
		//OnRaceFinish(EventArgs.Empty);
		finishMenu.SetActive (true);
		isFinish=true;
		infoText.text="";
		timerText.text="";
		((TextMesh)finalTime.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", timer.Minutes, timer.Seconds, timer.Milliseconds);;
	}

	// Update is called once per frame
	void Update () {
		if(!pause && !isFinish)
		{
			TimeSpan timeTemp = DateTime.Now.Subtract (startTime);

			timer = timer.Add (timeTemp);

			string formattedTimeSpan = string.Format("{0:D2}:{1:D2}:{2:D3}", timer.Minutes, timer.Seconds, timer.Milliseconds);
			timerText.text = formattedTimeSpan;
		}

		startTime = DateTime.Now;
	}
}
