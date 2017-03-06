using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseFunctionnality : MonoBehaviour {

	public GameObject Chrono;
	Text _chrono;
	public GameObject Main;
	public GameObject HUD;
	HUD _hudScript;

	public float Timer = 3;
	float _timer;
	bool _countdown = false;

	SubscriberList _subscriberList = new SubscriberList();

	void Awake () {
		_hudScript = HUD.GetComponent <HUD> ();
		_chrono = Chrono.GetComponent <Text> ();
		_timer = Timer;
	}
		
	void OnEnable () {
		_countdown = false;
		Main.SetActive (true);
		Chrono.SetActive (false);
	}

	void Update () {
		if (_countdown)
		{
			_timer -= Time.deltaTime;
			_chrono.text = Mathf.RoundToInt (_timer).ToString ();

			if (_timer <= -0.25f) {
				Event<PauseRingEvent>.Broadcast(new PauseRingEvent(false));
				Event<PausePlayerEvent>.Broadcast(new PausePlayerEvent(false));
				Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.PLAY));
				gameObject.SetActive (false);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnPauseGame ();
		}
	}

	public void UnPauseGame () {
		Main.SetActive (false);
		_countdown = true;
		_timer = Timer;
		Chrono.SetActive (true);
	}

	public void GoToMainMenu () {
		Event<PauseRingEvent>.Broadcast(new PauseRingEvent(false));
		Event<PausePlayerEvent>.Broadcast(new PausePlayerEvent(false));
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.MAIN));
		G.Sys.gameManager.GoToStartMenu ();
		HUD.SetActive (false);
		gameObject.SetActive (false);
	}

	public void ReStart () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		G.Sys.gameManager.RestartGame ();

		_hudScript.Coins = 0;
		_hudScript.Score = 0;

		_hudScript.ScoreText.text = "0";
		_hudScript.CoinText.text = "0";

		Event<PauseRingEvent>.Broadcast(new PauseRingEvent(false));
		Event<PausePlayerEvent>.Broadcast(new PausePlayerEvent(false));
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.PLAY));

		gameObject.SetActive (false);
	}
}
