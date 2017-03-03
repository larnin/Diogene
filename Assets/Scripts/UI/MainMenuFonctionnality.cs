using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.UI;

public class MainMenuFonctionnality : MonoBehaviour {

	public GameObject Credits;
	public GameObject Options;
	public GameObject MyHUD;

	public void GoToCredits () {
		Credits.SetActive (true);
		gameObject.SetActive (false);
	}

	public void GoToOptions () {
		Options.SetActive (true);
		gameObject.SetActive (false);
	}

	void Start () {

		G.Sys.dataMaster.ScreenSleepTime = Screen.sleepTimeout;

		if (!Directory.Exists("Saves")) {
			Directory.CreateDirectory("Saves");

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Create("Saves/save.diogene");

			G.Sys.dataMaster._save = new Save ();

			formatter.Serialize(saveFile, new Save ());

			saveFile.Close();
		}
		else {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Open("Saves/save.diogene", FileMode.Open);

			G.Sys.dataMaster._save = (Save)formatter.Deserialize(saveFile);

			saveFile.Close();

			G.Sys.dataMaster.SetEverything ();
		}
	}

	public void StartGame () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		G.Sys.gameManager.StartGame ();
		MyHUD.SetActive (true);
		gameObject.SetActive (false);
	}
}
