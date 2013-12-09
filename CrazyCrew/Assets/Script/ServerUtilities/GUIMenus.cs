using UnityEngine;
using System.Collections;

public static class GUIMenus
{
	public static void masterServerError(bool enabled)
	{
		MeshRenderer masterServerError = (MeshRenderer)GameObject.Find ("ConnectionErrorText").GetComponent("MeshRenderer");
		masterServerError.enabled = enabled;
	}

	public static void mainMenu(bool enabled)
	{
		MeshRenderer newGame = (MeshRenderer) GameObject.Find("NewGameText").GetComponent("MeshRenderer");
		MeshRenderer quit = (MeshRenderer) GameObject.Find("QuitText").GetComponent("MeshRenderer");

		newGame.enabled = enabled;
		quit.enabled = enabled;
	}

	public static void waitingForPlayersMenu(bool enabled)
	{
		MeshRenderer waiting = (MeshRenderer) GameObject.Find ("WaitingText").GetComponent("MeshRenderer");
		MeshRenderer playerCount = (MeshRenderer) GameObject.Find ("PlayerCountText").GetComponent("MeshRenderer");

		waiting.enabled = enabled;
		playerCount.enabled = enabled;
	}
}
