using UnityEngine;
using System.Collections;
using System;

public class BogieCarMovement : MonoBehaviour {

	private ServerBogieCar serverBogieCar;
	
	//Conponenti veicolo

	public int timeTorque = 2;

	//sterzo
	float lowestSteerAtSpeed = 50;
	float lowSpeedSteerAngle = 10;
	float highSpeedSteerAngle = 1;

	//ruote
	public WheelCollider WheelFR;
	public WheelCollider WheelFL;
	public WheelCollider WheelRR;
	public WheelCollider WheelRL;
	public WheelCollider WheelTraction;

	public Transform WheelFLTransform;
	public Transform WheelFRTransform;
	public Transform WheelRLTransform;
	public Transform WheelRRTransform;

	public float WheelTractionRPM;

	public float torqueTraction;
	public float brakeTorque;

	public float currentSpeed;

	//freni
	private bool brakeR =false;
	private bool brakeL =false;

	//valori fisici
	public float decelerationSpeed = -50f;
	public float retroSpeed=-10f;
	public float torqueMax=50f;
	public float topRetroSpeed=-10f;
	public float topSpeed=150f;

	//CheckPoint
	private bool checkPoint = false;
	public GameObject raceManager;


	private bool startRace = true;
	private bool finishRace = false;

	//bonus info
	public float speedMultiplierBonus;
	private Bonus bonusAcquired;

	//malus info
	private bool malusActive = false;
	private int reductionCounter = 0;
	private int malusDuration;

	private int malusSteerRotation=0;
	public float malusSlowdownMultiplier;

	public TextMesh infoText;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
		serverBogieCar = (ServerBogieCar) GameObject.Find ("Server").GetComponent("ServerBogieCar");
	}

	public Bonus getBonus()
	{
		return bonusAcquired;
	}

	/*public void setSteerMalusDuration()
	{
	}*/

	// Update is called once per frame
	void Update () {
		if(startRace && !finishRace)
			//Ora comincia la partita.
		{
			torqueTraction = WheelTraction.motorTorque;
			brakeTorque=WheelTraction.brakeTorque;
			currentSpeed= 2*22/7*WheelTraction.radius*WheelTraction.rpm*60/1000;
			currentSpeed=Mathf.Round(currentSpeed);

			//controllo ogni volta se entrambi i freni sono premuti ed in tal caso freno il veicolo
		Brake ();

		WheelRotate();
		}
	}

	public void WheelRotate()
	{
		WheelTractionRPM = WheelTraction.rpm;

		WheelFRTransform.Rotate(0,-WheelFR.rpm/60*360*Time.deltaTime,0);
		WheelRLTransform.Rotate(0,-WheelRL.rpm/60*360*Time.deltaTime,0);
		WheelRRTransform.Rotate(0,-WheelRR.rpm/60*360*Time.deltaTime,0);
		WheelFLTransform.Rotate(0,-WheelRR.rpm/60*360*Time.deltaTime,0);
		WheelFLTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,WheelFL.steerAngle-WheelFLTransform.localEulerAngles.z+90,WheelFLTransform.localEulerAngles.z);
		WheelFRTransform.localEulerAngles=new Vector3(WheelFLTransform.localEulerAngles.x,WheelFR.steerAngle-WheelFLTransform.localEulerAngles.z+90,WheelFLTransform.localEulerAngles.z);

	}
	
	public void BrakeROn()
	{
		brakeR=true;
	}
	
	public void BrakeROff()
	{
		brakeR=false;
	}
	
	public void BrakeLOn()
	{
		brakeL=true;
	}
	
	public void BrakeLOff()
	{
		brakeL=false;
	}

	/*
    * In caso di freno sinistro e destro premuto aziono il freno del veicolo
    */
	private void Brake()
	{		
		if(brakeL && brakeR) 
		{
			if(currentSpeed>0)
			{

				WheelTraction.motorTorque = 0;
				WheelTraction.brakeTorque = -decelerationSpeed;
			}
			else if(currentSpeed<=0 && currentSpeed>=topRetroSpeed)
			{
				WheelTraction.brakeTorque = 0;
				WheelTraction.motorTorque = retroSpeed;
			}
			else if(currentSpeed<=0 && currentSpeed<=topRetroSpeed)
			{
				WheelTraction.brakeTorque = 0;
				WheelTraction.motorTorque = 0;
			}
		}
		else 
		{
			WheelTraction.brakeTorque = 0;
		}
	}
	
	/// <summary>
	///  Indica quanto hai abbasato la leva sullo Smartphone
	///  rispetto al massimo possibile
	/// </summary>
	/// <param name="forcePercent">Force percent è compreso tra 0 e 1.</param>
	public void LeverDown(float forcePercent)
	{
		if(currentSpeed<topSpeed)
		{
			WheelTraction.motorTorque += forcePercent*torqueMax;
			StartCoroutine (StopTorque(forcePercent*torqueMax));
		}
		else
		{
			WheelTraction.motorTorque = 0;
		}
	}

	IEnumerator StopTorque(float torque)
	{
		yield return new WaitForSeconds(timeTorque);

		if(WheelTraction.motorTorque > 0)
		{
			if(WheelTraction.motorTorque < torque)
			{
				WheelTraction.motorTorque = 0;
			}
			else
			{
				WheelTraction.motorTorque -= torque;
			}
		}

	}

	public void Steer(float steerPercent)
	{
		float speedFactor = rigidbody.velocity.magnitude/lowestSteerAtSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);

		currentSteerAngle = (currentSteerAngle*steerPercent) + malusSteerRotation;

		
		WheelFL.steerAngle = currentSteerAngle;
		WheelFR.steerAngle = currentSteerAngle;
	}
	
	IEnumerator Finish ()
	{
		Debug.Log ("finish");

		if(!finishRace)
		{
			finishRace = true;
			yield return 0;
			WheelTraction.brakeTorque=1000;
			((RaceManager)raceManager.GetComponent ("RaceManager")).FinishLine ();
		}
	}

	//Collisione con oggetti in gara
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.name == "BonusObject")
		{
			int powerUpId = UnityEngine.Random.Range(1,5);
			serverBogieCar.bonusComunication(powerUpId);
			switch(powerUpId)
			{
			case 1:
			{
				bonusAcquired = new BonusMissile();
				break;
			}
				
			case 2:
			{
				bonusAcquired = new BonusPoop();
				break;
			}
				
			case 3:
			{
				StartCoroutine(BonusTime());
				break;
			}
				
			case 4:
			{
				bonusAcquired = new BonusSpeed();
				break;	
			}
			}
		}
		else
		{
			if(other.gameObject.name == "FinishLine")
			{
				//Finish si attiva solo se checkPoint = true
				if(checkPoint)
				{
					StartCoroutine (Finish());
				}
			}
			else
			{
				if(other.gameObject.name == "CheckPoint")
				{
					checkPoint = !checkPoint;
				}
				else
				{
					if(other.gameObject.name == "MalusObject" && !malusActive)
					{
						malusActive = true;

						int powerUpId = UnityEngine.Random.Range(1,5);

						powerUpId=2;
						serverBogieCar.malusComunication(powerUpId);
						switch(powerUpId)
						{
						case 1:
						{
							//effetto fango su schermo
							serverBogieCar.malusEnded();
							malusActive = false;
							break;
						}
							
						case 2:
						{
							StartCoroutine(MalusSlowdown());
							break;
						}

						case 3:
						{
							serverBogieCar.assignRoles();
							malusActive = false;
							break;
						}
							
						case 4:
						{
							StartCoroutine(MalusSteerFailure());
							break;	
						}
						}
					}
				}
			}
		}
	}


	//GESTIONE POWER-UP
	//BONUS

	public void bonusSpeed()
	{
		Debug.Log ("Turbo attivato");
		StartCoroutine(BonusSpeed());
	}

	IEnumerator BonusSpeed()
	{
		infoText.text="TURBOOOOOO";
		WheelTraction.motorTorque += torqueMax*speedMultiplierBonus;
		yield return new WaitForSeconds(2);
		StartCoroutine (StopTorque(torqueMax*speedMultiplierBonus));
		infoText.text="";
	}

	//powerUp: il tempo viene decrementato di un valore tra 5 e 10 secondi
	IEnumerator BonusTime()
	{
		int bonusTime = UnityEngine.Random.Range(5,11);
		infoText.text="BONUS TIME: " +bonusTime+ "sec";
		((RaceManager)raceManager.GetComponent ("RaceManager")).bonusTime(bonusTime);
		yield return new WaitForSeconds(3);
		infoText.text="";
	}


	//MALUS
	public void malusReduction(string malusName)
	{
		reductionCounter++;

		if(reductionCounter % 3 == 0)
		{
			malusDuration--;
			StartCoroutine(MalusReductionNotify());
		}
	}

	IEnumerator MalusReductionNotify()
	{
		infoText.text=" Malus reduced...GOOD JOB!";
		yield return new WaitForSeconds(2);
		infoText.text="";
	}

	IEnumerator MalusSteerFailure()
	{
		malusDuration=5;
		malusSteerRotation=6;

		if(UnityEngine.Random.Range(1,2) == 1)
		{
			malusSteerRotation = -malusSteerRotation;
		}
	
		for(int i=0;i<malusDuration;i++)
		{
			infoText.text="STEER FAILURE! Press the button to reduce it!";
			yield return new WaitForSeconds(1);
		}

		serverBogieCar.malusEnded();
		malusDuration=5;
		malusSteerRotation=0;
		infoText.text="";
		malusActive = false;
	}

	IEnumerator MalusSlowdown()
	{
		malusDuration=5;
		WheelTraction.brakeTorque = -decelerationSpeed*malusSlowdownMultiplier;

		for(int i=0;i<malusDuration;i++)
		{
			infoText.text="UNEXPECTED SLOWDOWN! Press the button to reduce it!";
			yield return new WaitForSeconds(1);
		}
		WheelTraction.brakeTorque = 0;

		serverBogieCar.malusEnded();
		malusDuration=5;
		infoText.text="";
		malusActive = false;
	}
}