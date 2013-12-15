using UnityEngine;
using System.Collections;

public static class GUIMenusServer
{
	public static void masterServerError(bool enabled)
	{
		GameObject masterServerError = GameObject.Find ("ConnectionErrorText");
		((MeshRenderer) masterServerError.GetComponent("MeshRenderer")).enabled = enabled;
	}

	public static void mainMenu(bool enabled)
	{
		GameObject newGame = GameObject.Find("NewGameText");
		GameObject quit = GameObject.Find("QuitText");

		MeshRenderer mesh = (MeshRenderer) newGame.GetComponent("MeshRenderer");
		BoxCollider collider = (BoxCollider) newGame.GetComponent("BoxCollider");

		mesh.enabled = enabled;
		collider.enabled = enabled;

		mesh = (MeshRenderer) quit.GetComponent("MeshRenderer");
		collider = (BoxCollider) quit.GetComponent("BoxCollider");

		mesh.enabled = enabled;
		collider.enabled = enabled;
	}

	public static void waitingForPlayersMenu(bool enabled)
	{
		GameObject waiting = GameObject.Find("WaitingText");
		GameObject playerCount = GameObject.Find("PlayerCountText");
		
		((MeshRenderer) waiting.GetComponent("MeshRenderer")).enabled = enabled;
		((MeshRenderer) playerCount.GetComponent("MeshRenderer")).enabled = enabled;
	}
}