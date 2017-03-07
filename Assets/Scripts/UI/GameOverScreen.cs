using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class GameOverScreen : MonoBehaviour {

	public string MenuMusic;
	[SoundGroupAttribute] public string NewRecordSound;

	public Text Score;
	public Text CoinsGained;
	public GameObject NewRecord;
	public GameObject HUD;
	HUD hudScript;

	// Use this for initialization
	void Awake () {
		hudScript = HUD.GetComponent <HUD>();
	}

	void OnEnable () {

		Screen.sleepTimeout = G.Sys.dataMaster.ScreenSleepTime;

		Score.text = hudScript.ScoreText.text;
		if (hudScript.Score > G.Sys.dataMaster.HighScore) {
			G.Sys.dataMaster.HighScore = hudScript.Score;
			Event<PlaySoundEvent>.Broadcast(new PlaySoundEvent(NewRecordSound));
			NewRecord.SetActive (true);
		}
		else {
			NewRecord.SetActive (false);
		}

		G.Sys.dataMaster.Coins += hudScript.Coins;

		CoinsGained.text = "+ " + hudScript.CoinText.text;

		Event<GameOverEvent>.Broadcast(new GameOverEvent(hudScript.Score, hudScript.Coins));
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.GAMEOVER));

		HUD.SetActive (false);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GoToMainMenu ();
		}
	}

	public void GoToMainMenu () {

		Event<ChangeMusicEvent>.Broadcast(new ChangeMusicEvent(MenuMusic));

		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.MAIN));
		G.Sys.gameManager.GoToStartMenu ();
		gameObject.SetActive (false);
	}

	public void ReStart () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		G.Sys.gameManager.RestartGame ();

		hudScript.Coins = 0;
		hudScript.Score = 0;

		hudScript.ScoreText.text = "0";
		hudScript.CoinText.text = "0";

		hudScript.CanPause = true;

		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.PLAY));

		gameObject.SetActive (false);
	}
}
