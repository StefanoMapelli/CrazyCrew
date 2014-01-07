using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

	private NetworkView networkView;
	private string role;
	private TextMesh text;

	//valori aggiornati ogni qualvolta viene acquisito/utilizzato/rimosso un bonus o un malus tremite RPC dal server
	private bool hasBonus = false;
	private bool hasMalus = false;

	// Use this for initialization
	void Start () {
		GameObject client = GameObject.Find ("Client");
		networkView = (NetworkView) client.GetComponent("NetworkView");
	}

	public void setRole(string role)
	{
		this.role = role;
	}

	public void setHasBonus(bool hasBonus)
	{
		this.hasBonus = hasBonus;
	}

	public void setHasMalus(bool hasMalus)
	{
		this.hasMalus = hasMalus;
	}

	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown()
	{
		if(role == "Lever1" && hasBonus)
		{
			networkView.RPC("activateBonus", RPCMode.Server);
			Debug.Log("Sono: "+role+" e ho premuto il PowerUpButton");
		}
		else
		{
			if(role == "Lever2" && hasMalus)
			{
				networkView.RPC("reduceMalus", RPCMode.Server);
				Debug.Log("Sono: "+role+" e ho premuto il PowerUpButton");
			}
		}
	}
}
