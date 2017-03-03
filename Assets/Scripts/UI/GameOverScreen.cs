using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	public Text CoinsGained;
	public GameObject NewRecord;
	public GameObject HUD;
	HUD hudScript;

	// Use this for initialization
	void Start () {
		hudScript = HUD.GetComponent <HUD>();
	}

	void OnEnable () {

		Screen.sleepTimeout = G.Sys.dataMaster.ScreenSleepTime;

		if (hudScript.Score > G.Sys.dataMaster.HighScore) {
			G.Sys.dataMaster.HighScore = hudScript.Score;
			NewRecord.SetActive (false);
		}
		else {
			NewRecord.SetActive (true);
		}

		G.Sys.dataMaster.Coins += hudScript.Coins;

		CoinsGained.text = hudScript.CoinText.text;

		HUD.SetActive (false);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GoToMainMenu ();
		}
	}

	public void GoToMainMenu () {
		G.Sys.gameManager.GoToStartMenu ();
		gameObject.SetActive (false);
	}

	public void ReStart () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		G.Sys.gameManager.RestartGame ();
		gameObject.SetActive (false);
	}
}
