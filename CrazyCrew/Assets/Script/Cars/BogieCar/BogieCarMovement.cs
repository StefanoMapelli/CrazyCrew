using UnityEngine;
using System.Collections;
using System;

public class BogieCarMovement : MonoBehaviour {

	//Conponenti veicolo

	public int timeTorque = 2;

	//sterzo
	float lowestSteerAtSpeed = 50;
	float lowSpeedSteerAngle = 10;
	float highSpeedSteerAngle = 1;
	bool isSteerAvailable=true;

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

	//power up
	private float powerUpSpeed=1f;

	//CheckPoint
	private bool checkPoint = false;
	public GameObject raceManager;


	private bool startRace = true;
	private bool finishRace = false;

	public TextMesh infoText;
	
	// Use this for initialization
	void Start () {

		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);

	}

	
	// Update is called once per frame
	void Update () {
		if(startRace && !finishRace)
			//Ora comincia la partita.
		{
			Debug.Log("freno retro destro"+ WheelRR.brakeTorque);
			Debug.Log("freno retro sinistra "+ WheelRL.brakeTorque);
			torqueTraction = WheelTraction.motorTorque;
			brakeTorque=WheelTraction.brakeTorque;
			//currentSpeed= 2*22/7*WheelRL.radius*WheelRL.rpm*60/1000;
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
				Debug.Log ("dec" + decelerationSpeed);

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
		Debug.Log("Lever  ");
		if(currentSpeed<topSpeed)
		{
			/*WheelRL.motorTorque= forcePercent*torqueMax*powerUpSpeed;
			WheelRR.motorTorque= forcePercent*torqueMax*powerUpSpeed;*/

			WheelTraction.motorTorque += forcePercent*torqueMax*powerUpSpeed;
			Debug.Log("Lever  "+forcePercent*torqueMax*powerUpSpeed);
			StartCoroutine (StopTorque(forcePercent*torqueMax*powerUpSpeed));

		}
		else
		{
			/*WheelRL.motorTorque= 0;
			WheelRR.motorTorque= 0;*/
			WheelTraction.motorTorque = 0;
		}
	}

	IEnumerator StopTorque(float torque)
	{
		yield return new WaitForSeconds(timeTorque);

		/*if(WheelRL.motorTorque > 0 && WheelRR.motorTorque > 0)
		{
			if(WheelRL.motorTorque < torque || WheelRR.motorTorque < torque)
			{
				WheelRL.motorTorque = 0;
				WheelRR.motorTorque = 0;
				WheelTraction.motorTorque = 0;
			}
			else
			{
				WheelRL.motorTorque -= torque;
				WheelRR.motorTorque -= torque;
			}
		}*/
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
		if(isSteerAvailable)
		{
			float speedFactor = rigidbody.velocity.magnitude/lowestSteerAtSpeed;
			float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);

			currentSteerAngle *= steerPercent;
		
			WheelFL.steerAngle = currentSteerAngle;
			WheelFR.steerAngle = currentSteerAngle;
		}
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

	//Collisione con powerUp
	void OnTriggerEnter(Collider other) 
	{
		
		if(other.gameObject.name == "PowerUpObject")
		{
			int powerUpId= UnityEngine.Random.Range(1,4);
			switch(powerUpId)
			{
				case 1:
				{
					//doppia spinta
					StartCoroutine (DoubleSpeedStart());
					break;
				}

				case 2:
				{
					//sterzo bloccato
					StartCoroutine (SteerBlock());
					break;
				}

				case 3:
				{
					//tempo decrementato
					StartCoroutine (BonusTime());
					break;
				}
				//manca il cmabio dei controlli
			}
		}
		else if(other.gameObject.name == "FinishLine")
		{
			//Finish si attiva solo se checkPoint = true
			if(checkPoint)
			{
				StartCoroutine (Finish());
			}
		}
		else if(other.gameObject.name == "CheckPoint")
		{
			checkPoint = !checkPoint;
		}
		
		
	}

	//powerup: sterzo non funzionante per 3 secondi
	IEnumerator SteerBlock()
	{
		infoText.text="OPS...STEER BLOCKED";
		isSteerAvailable=false;
		yield return new WaitForSeconds(3);
		infoText.text="";
		isSteerAvailable=true;

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

	//powerup che raddoppia la forza data dalla leva
	IEnumerator DoubleSpeedStart()
	{
		powerUpSpeed=2f;
		infoText.text="DOUBLE FORCE!!!";
		yield return new WaitForSeconds(5);
		infoText.text="";
		powerUpSpeed=1f;
	
	
	}
}
