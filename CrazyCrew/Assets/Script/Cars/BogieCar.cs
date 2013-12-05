using UnityEngine;
using System.Collections;
using System.Threading;

public class BogieCar : MonoBehaviour {

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

	public float WheelFRRPM;
	public float WheelFLRPM;
	public float WheelRRRPM;
	public float WheelRLRPM;

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

	// Use this for initialization
	void Start () {
		rigidbody.centerOfMass=new Vector3(0,-0.2f,0);
	}

	
	// Update is called once per frame
	void Update () {

		currentSpeed= 2*22/7*WheelRL.radius*WheelRL.rpm*60/1000;
		currentSpeed=Mathf.Round(currentSpeed);

		WheelFRRPM = WheelFR.rpm;
		WheelFLRPM = WheelFL.rpm;
		WheelRRRPM = WheelRR.rpm;
		WheelRLRPM = WheelRL.rpm;

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
				WheelRL.brakeTorque = -decelerationSpeed;
				WheelRR.brakeTorque = -decelerationSpeed;
				WheelRL.motorTorque = 0;
				WheelRR.motorTorque = 0;
			}
			else if(currentSpeed<=0 && currentSpeed>=topRetroSpeed)
			{
				WheelRL.brakeTorque = 0;
				WheelRR.brakeTorque = 0;
				WheelRL.motorTorque = retroSpeed;
				WheelRR.motorTorque = retroSpeed;
			}
			else if(currentSpeed<=0 && currentSpeed<=topRetroSpeed)
			{
				WheelRL.brakeTorque = 0;
				WheelRR.brakeTorque = 0;
				WheelRL.motorTorque = 0;
				WheelRR.motorTorque = 0;
			}
		}
		else 
		{
			WheelRL.brakeTorque = 0;
			WheelRR.brakeTorque = 0;
		}
	}

	/*
         * Force percent è compreso tra 0 e 1
         * Indica quanto hai abbasato la leva sullo Smartphone
         * rispetto al massimo possibile
         *
         */
	public void LeverDown(float forcePercent)
	{
		Debug.Log("Lever  ");
		if(currentSpeed<topSpeed)
		{
			WheelRL.motorTorque= forcePercent*torqueMax;
			WheelRR.motorTorque= forcePercent*torqueMax;

			StartCoroutine (StopTorque(forcePercent*torqueMax));

		}
		else
		{
			WheelRL.motorTorque= 0;
			WheelRR.motorTorque= 0;
		}
	}

	IEnumerator StopTorque(float torque)
	{
		yield return new WaitForSeconds(timeTorque);

		if(WheelRL.motorTorque > 0 && WheelRR.motorTorque > 0)
		{
			if(WheelRL.motorTorque < torque || WheelRR.motorTorque < torque)
			{
				WheelRL.motorTorque = 0;
				WheelRR.motorTorque = 0;
			}
			else
			{
				WheelRL.motorTorque -= torque;
				WheelRR.motorTorque -= torque;
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
}
