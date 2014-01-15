using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RaceManager : MonoBehaviour {

	private BogieCarMovement bogieCarMovement;

	//evento fine gara
	public delegate void RaceFinishEventHandler(object sender, EventArgs e);
	public event EventHandler RaceFinish;

	//info
	private TextMesh infoText;
	private TextMesh rankingText;
	public GameObject camera;

	//Pause
	private bool pause = true;

	//timer
	private DateTime startTime;
	TimeSpan timer = new TimeSpan(0,0,0,0,0);
	private TextMesh timerText;

	//Menu finale
	public GameObject finishMenu;
	public GameObject time1;
	public GameObject time2;
	public GameObject time3;
	public GameObject time4;
	public GameObject time5;
	public GameObject time6;

	//Audio
	public AudioSource countdownSound;
	public AudioSource pauseMusic;
	public AudioSource gameMusic;
	public AudioSource finishMusic;

	private bool isFinish = false;

	//Opponent car object
	private AICarScript opponentCar1;
	private AICarScript opponentCar2;

	// Use this for initialization
	void Start () {
		//CountDown
		StartCoroutine (StartGame ());
	}

	protected virtual void OnRaceFinish(EventArgs e)
	{
		RaceFinish(this,e);
	}

	public void RestartRace () {
		//CountDown
		isFinish=false;
		((CarCamera)GameObject.Find("Camera").GetComponent("CarCamera")).restartCamera();
		StartCoroutine (StartGame ());
	}

	/// <summary>
	/// Set up timer, manage initial countDown.
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartGame()
	{
		gameMusic.Play();
		finishMusic.Stop ();

		bogieCarMovement = (BogieCarMovement) GameObject.Find("_BogieCarModel").GetComponent("BogieCarMovement");
		infoText =  (TextMesh) (GameObject.Find("InfoText").GetComponent("TextMesh"));
		rankingText =  (TextMesh) (GameObject.Find("RankingText").GetComponent("TextMesh"));
		infoText.text="";

		countdownSound.Play();
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

		// just an attempt
		ServerBogieCar serverBogieCar = (ServerBogieCar) GameObject.Find ("Server").GetComponent("ServerBogieCar");
		serverBogieCar.assignRoles();
		// just an attempt

		opponentCar1= (AICarScript) GameObject.Find("_OpponentCarModel1").GetComponent("AICarScript");
		opponentCar2= (AICarScript) GameObject.Find("_OpponentCarModel2").GetComponent("AICarScript");

		opponentCar1.setRaceStarted(true);
		opponentCar2.setRaceStarted(true);

		yield return new WaitForSeconds(1);
		infoText.text = "";
	}

	public void SetPause(bool pause)
	{
		this.pause = pause;

		if(pause)
		{
			gameMusic.Stop();
			pauseMusic.Play ();
		}
		else
		{
			gameMusic.Play();
			pauseMusic.Stop();
		}
	}

	/// <summary>
	/// Il veicolo è giunto alla fine del percorso.
	/// </summary>
	public void FinishLine()
	{
		OnRaceFinish(EventArgs.Empty);
		finishMenu.SetActive (true);
		isFinish=true;
		infoText.text="";
		timerText.text="";
		ArrayList ranking=Ranking();
		bool isInRankingText=false;
		int rank=1;
		for(int i=0;i<ranking.Count;i++)
		{
			if(!isInRankingText)
			{
				//se il tempo di bogie car è minore dell'avversario scrivo a video il tempo di bogie car in classifica
				if(((AICarScript)ranking[i]).finalTime.CompareTo(bogieCarMovement.finalTime)>0)
				{
					switch(rank)
					{
					case 1: 
					{
						((TextMesh)time1.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
						((TextMesh)time1.GetComponent ("TextMesh")).color=Color.yellow;
						break;
					}
					case 2: 
					{
						((TextMesh)time2.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
						((TextMesh)time2.GetComponent ("TextMesh")).color=Color.yellow;
						break;
					}
					case 3: 
					{
						((TextMesh)time3.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
						((TextMesh)time3.GetComponent ("TextMesh")).color=Color.yellow;
						break;
					}
					case 4: 
					{
						((TextMesh)time4.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
						((TextMesh)time4.GetComponent ("TextMesh")).color=Color.yellow;
						break;
					}
					case 5: 
					{
						((TextMesh)time5.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
						((TextMesh)time5.GetComponent ("TextMesh")).color=Color.yellow;
						break;
					}
					case 6: 
					{
						((TextMesh)time6.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
						((TextMesh)time6.GetComponent ("TextMesh")).color=Color.yellow;
						break;
					}
					}
					rank++;
					isInRankingText=true;
				}
			}
			switch(rank)
			{
			case 1: 
			{
				((TextMesh)time1.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", ((AICarScript)ranking[i]).finalTime.Minutes, ((AICarScript)ranking[i]).finalTime.Seconds, ((AICarScript)ranking[i]).finalTime.Milliseconds);
				break;
			}
			case 2: 
			{
				((TextMesh)time2.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", ((AICarScript)ranking[i]).finalTime.Minutes, ((AICarScript)ranking[i]).finalTime.Seconds, ((AICarScript)ranking[i]).finalTime.Milliseconds);
				break;
			}
			case 3: 
			{
				((TextMesh)time3.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", ((AICarScript)ranking[i]).finalTime.Minutes, ((AICarScript)ranking[i]).finalTime.Seconds, ((AICarScript)ranking[i]).finalTime.Milliseconds);
				break;
			}
			case 4: 
			{
				((TextMesh)time4.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", ((AICarScript)ranking[i]).finalTime.Minutes, ((AICarScript)ranking[i]).finalTime.Seconds, ((AICarScript)ranking[i]).finalTime.Milliseconds);
				break;
			}
			case 5: 
			{
				((TextMesh)time5.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", ((AICarScript)ranking[i]).finalTime.Minutes, ((AICarScript)ranking[i]).finalTime.Seconds, ((AICarScript)ranking[i]).finalTime.Milliseconds);
				break;
			}
			case 6: 
			{
				((TextMesh)time6.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", ((AICarScript)ranking[i]).finalTime.Minutes, ((AICarScript)ranking[i]).finalTime.Seconds, ((AICarScript)ranking[i]).finalTime.Milliseconds);
				break;
			}
			}
			rank++;
		}
		if(!isInRankingText)
		{
			switch(rank)
			{
			case 1: 
			{
				((TextMesh)time1.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
				((TextMesh)time1.GetComponent ("TextMesh")).color=Color.yellow;
				break;
			}
			case 2: 
			{
				((TextMesh)time2.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
				((TextMesh)time2.GetComponent ("TextMesh")).color=Color.yellow;
				break;
			}
			case 3: 
			{
				((TextMesh)time3.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
				((TextMesh)time3.GetComponent ("TextMesh")).color=Color.yellow;
				break;
			}
			case 4: 
			{
				((TextMesh)time4.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
				((TextMesh)time4.GetComponent ("TextMesh")).color=Color.yellow;
				break;
			}
			case 5: 
			{
				((TextMesh)time5.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
				((TextMesh)time5.GetComponent ("TextMesh")).color=Color.yellow;
				break;
			}
			case 6: 
			{
				((TextMesh)time6.GetComponent ("TextMesh")).text += string.Format("{0:D2}:{1:D2}:{2:D3}", bogieCarMovement.finalTime.Minutes, bogieCarMovement.finalTime.Seconds, bogieCarMovement.finalTime.Milliseconds);
				((TextMesh)time6.GetComponent ("TextMesh")).color=Color.yellow;
				break;
			}
			}
		}
		((CarCamera)camera.GetComponent("CarCamera")).cameraOnFinishMenu();
		gameMusic.Stop();
		pauseMusic.Stop();
		finishMusic.Play ();
	}

	public TimeSpan getFinalTime()
	{
		return timer;
	}

	public TimeSpan getTime()
	{
		return timer;
	}

	public ArrayList Ranking()
	{
		ArrayList cars=new ArrayList();
		cars.Add(opponentCar1);
		cars.Add(opponentCar2);
		object temp;

		for(int i=0;i<cars.Count;i++)
		{
			if(((AICarScript) cars[i]).finalTime.CompareTo(new TimeSpan(0,0,0))==0)
			{
				int h=0;
				int m=0;
				int s=UnityEngine.Random.Range(5,60);
				int ms=UnityEngine.Random.Range(0,99);

				((AICarScript) cars[i]).finalTime=bogieCarMovement.finalTime.Add(new TimeSpan(0,h,m,s,ms));
			}
		}

		for(int i=0;i<cars.Count;i++)
		{
			for(int j=1;i<cars.Count;i++)
			{
				if(((AICarScript) cars[j-1]).finalTime.CompareTo(((AICarScript) cars[j]).finalTime)>0)
				{
					temp=cars[j];
					cars[j]=cars[j-1];
					cars[j-1]=temp;
				}
			}
		}
		return cars;
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

	//da usare nel caso di bonus di tempo
	public void bonusTime(int bonus)
	{
		timer = timer.Subtract(new TimeSpan(0,0,bonus));
	}

	public void ShowRanking(Dictionary<string,TimeSpan> timeList)
	{

		StartCoroutine(ShowRankingText(timeList));
	}

	IEnumerator ShowRankingText(Dictionary<string,TimeSpan> timeList)
	{
		string text="Ranking:";
		foreach (string key in timeList.Keys)
		{
			if(key=="Player")
			{
				text+="\n" + key + "           "+ string.Format("{0:D2}:{1:D2}:{2:D3}", timeList[key].Minutes, timeList[key].Seconds, timeList[key].Milliseconds);
			}
			else
			{
				text+="\n" + key + "    "+ string.Format("{0:D2}:{1:D2}:{2:D3}", timeList[key].Minutes, timeList[key].Seconds, timeList[key].Milliseconds);
			}
		}
		rankingText.text=text;
		yield return new WaitForSeconds(5);
		rankingText.text="";

	}
}