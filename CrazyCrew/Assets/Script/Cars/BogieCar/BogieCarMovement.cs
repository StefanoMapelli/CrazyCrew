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

	public float WheelFRRPM;
	public float WheelFLRPM;
	public float WheelRRRPM;
	public float WheelRLRPM;
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

	private TextMesh PowerUpText;

	//Timer
	private TextMesh timerText;
	//Da settare quando comincia la partita
	private DateTime startTime;

	//TimeOut all'inizio della partita
	private TextMesh countDownText;

	// Use this for initialization
	void Start () {
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);

		PowerUpText =  (TextMesh) (GameObject.Find("PowerUpText").GetComponent("TextMesh"));
		PowerUpText.text="";

		//CountDown

		countDownText = (TextMesh) (GameObject.Find("CountDownText").GetComponent("TextMesh"));
		StartCoroutine( WriteTextMesh(countDownText, "3", 0));
		StartCoroutine( WriteTextMesh(countDownText, "2", 1));
		StartCoroutine( WriteTextMesh(countDownText, "1", 2));
		StartCoroutine( WriteTextMesh(countDownText, "Go!!!", 3));
		StartCoroutine( WriteTextMesh(countDownText, "", 4));

		timerText =  (TextMesh) (GameObject.Find("TimerText").GetComponent("TextMesh"));
		timerText.text = "00:00";
		startTime = DateTime.Now;
	}

	/// <summary>
	/// Wait for seconds time and wrtite text in textMesh.
	/// </summary>
	/// <param name="textMesh">Text mesh where write.</param>
	/// <param name="text">Text to write.</param>
	/// <param name="seconds">Seconds to wait before write.</param>
	private IEnumerator WriteTextMesh(TextMesh textMesh, string text, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		textMesh.text = text;
	}

	
	// Update is called once per frame
	void Update () {

		//Setto il time di partenza
		if(countDownText.text == "Go!!!")
		{
			startTime = DateTime.Now;
		}


		if(countDownText.text == "" || countDownText.text == "Go!!!")
			//Ora comincia la partita.
		{
			TimeSpan timeTemp = DateTime.Now - startTime;
			Debug.Log (timeTemp);
			timerText.text = timeTemp.Minutes.ToString () + ":" + timeTemp.Seconds.ToString()+ ":" + timeTemp.Milliseconds.ToString();
		
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
		WheelFRRPM = WheelFR.rpm;
		WheelFLRPM = WheelFL.rpm;
		WheelRRRPM = WheelRR.rpm;
		WheelRLRPM = WheelRL.rpm;
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


		StartCoroutine (DoubleSpeedStart());



	}

	//power up coroutine da implementare
	IEnumerator DoubleSpeedStart()
	{
		powerUpSpeed=2f;
		PowerUpText.text="Doppia Velocità...CANE!!!";
		yield return new WaitForSeconds(2);
		PowerUpText.text="";
		yield return new WaitForSeconds(3);

		powerUpSpeed=1f;
	}
}
