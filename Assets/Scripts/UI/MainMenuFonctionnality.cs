using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class MainMenuFonctionnality : MonoBehaviour {

	public string GameMusic;

	public Text Score;
	public GameObject Credits;
	public GameObject Options;
	public GameObject MyHUD;
	public GameObject Shop;

	public void GoToCredits () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.CREDITS));
		Credits.SetActive (true);
		gameObject.SetActive (false);
	}

	public void GoToOptions () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.OPTIONS));
		Options.SetActive (true);
		gameObject.SetActive (false);
	}

	public void GoToShop () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.SHOP));
		Shop.SetActive (true);
		gameObject.SetActive (false);
	}

	public void OnEnable() {
		Score.text = G.Sys.dataMaster.HighScoreText;
	}

	public void StartGame () {
		Event<ChangeMusicEvent>.Broadcast(new ChangeMusicEvent(GameMusic));

		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.PLAY));
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		G.Sys.gameManager.StartGame ();
		MyHUD.SetActive (true);
		gameObject.SetActive (false);
	}
}
