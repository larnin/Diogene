using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GoToMainMenu ();
		}
	}

	public void GoToMainMenu () {
		G.Sys.gameManager.GoToStartMenu ();
	}

	public void ReStart () {
		G.Sys.gameManager.RestartGame ();
	}
}
