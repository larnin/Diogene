using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class MainMenuFonctionnality : MonoBehaviour {

	[SoundGroupAttribute] public string MenuMusic;
	[SoundGroupAttribute] public string GameMusic;

	public Text Score;
	public GameObject Credits;
	public GameObject Options;
	public GameObject MyHUD;

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

	void Awake () {

		G.Sys.dataMaster.ScreenSleepTime = Screen.sleepTimeout;

		if (!File.Exists(Application.persistentDataPath + "/save.diogene")) {

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Create(Application.persistentDataPath + "/save.diogene");

			G.Sys.dataMaster._save = new Save ();

			formatter.Serialize(saveFile, new Save ());

			saveFile.Close();
		}
		else {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Open(Application.persistentDataPath + "/save.diogene", FileMode.Open);

			G.Sys.dataMaster._save = (Save)formatter.Deserialize(saveFile);

			saveFile.Close();

			G.Sys.dataMaster.SetEverything ();
		}
	}

	public void OnEnable() {
		Score.text = G.Sys.dataMaster.HighScoreText;
	}

	public void StartGame () {
		Event<StopSoundEvent>.Broadcast(new StopSoundEvent(MenuMusic));
		Event<PlaySoundEvent>.Broadcast(new PlaySoundEvent(GameMusic));

		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.PLAY));
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		G.Sys.gameManager.StartGame ();
		MyHUD.SetActive (true);
		gameObject.SetActive (false);
	}
}
