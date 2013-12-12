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
	public float torqueCorrectionL=1f;
	public float torqueCorrectionR=1f;

	//power up
	private float powerUpSpeed=1f;

	//CheckPoint
	private bool checkPoint = false;
	public GameObject raceManager;


	private bool startRace = false;
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
			torqueTraction = WheelTraction.motorTorque;
			//currentSpeed= 2*22/7*WheelRL.radius*WheelRL.rpm*60/1000;
			currentSpeed= 2*22/7*WheelTraction.radius*WheelTraction.rpm*60/1000;
			currentSpeed=Mathf.Round(currentSpeed);

			if(WheelFR.rpm<WheelFL.rpm)
			{
				torqueCorrectionL=0.90f;
				torqueCorrectionR=1f;
				
			}
			else if(WheelFL.rpm<WheelFR.rpm)
			{
				torqueCorrectionR=0.90f;;
				torqueCorrectionL=1f;
				
				
			}
			else if(WheelFL.rpm==WheelFR.rpm)
			{
				torqueCorrectionR=1f;;
				torqueCorrectionL=1f;
			}

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
				/*WheelRL.brakeTorque = -decelerationSpeed;
				WheelRR.brakeTorque = -decelerationSpeed;
				WheelRL.motorTorque = 0;
				WheelRR.motorTorque = 0;*/
				WheelTraction.motorTorque = 0;
				WheelTraction.brakeTorque = -decelerationSpeed;
			}
			else if(currentSpeed<=0 && currentSpeed>=topRetroSpeed)
			{
				/*WheelRL.brakeTorque = 0;
				WheelRR.brakeTorque = 0;
				WheelRL.motorTorque = retroSpeed;
				WheelRR.motorTorque = retroSpeed;*/
				WheelTraction.brakeTorque = 0;
				WheelTraction.motorTorque = retroSpeed;
			}
			else if(currentSpeed<=0 && currentSpeed<=topRetroSpeed)
			{
				/*WheelRL.brakeTorque = 0;
				WheelRR.brakeTorque = 0;
				WheelRL.motorTorque = 0;
				WheelRR.motorTorque = 0;*/

				WheelTraction.brakeTorque = 0;
				WheelTraction.motorTorque = 0;
			}
		}
		else 
		{
			WheelTraction.brakeTorque = 0;
/*			WheelRL.brakeTorque = 0;
			WheelRR.brakeTorque = 0;*/
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
		float speedFactor = rigidbody.velocity.magnitude/lowestSteerAtSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);

		currentSteerAngle *= steerPercent;
		
		WheelFL.steerAngle = currentSteerAngle;
		WheelFR.steerAngle = currentSteerAngle;
	}

	//Collisione con powerUp
	void OnTriggerEnter(Collider other) {

		if(other.gameObject.name == "PowerUpObject")
		{
			StartCoroutine (DoubleSpeedStart());
		}else if(other.gameObject.name == "FinishLine")
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

	IEnumerator Finish ()
	{
		Debug.Log ("finish");

		if(!finishRace)
		{
			finishRace = true;
			yield return 0;
			Time.timeScale = 0;
			((RaceManager)raceManager.GetComponent ("RaceManager")).FinishLine ();
		}
	}

	//power up coroutine da implementare
	IEnumerator DoubleSpeedStart()
	{
		powerUpSpeed=2f;
		infoText.text="Doppia Velocità!!!";
		yield return new WaitForSeconds(2);
		infoText.text="";
		yield return new WaitForSeconds(3);

		powerUpSpeed=1f;
	
	
	}

	/// <summary>
	/// Restart the race.
	/// Sposto il veicolo alla partenza
	/// </summary>
	public void RestartRace()
	{
		transform.position = new Vector3(14,0,114);
	}
}
