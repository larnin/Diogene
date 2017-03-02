using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.UI;

public class MainMenuFonctionnality : MonoBehaviour {

	public GameObject Credits;

	public Text BestScoreText;
	public GameObject BestScoreImage;

	public void GoToCredits () {
		Credits.SetActive (true);
		gameObject.SetActive (false);
	}

	void Start () {
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

	void OnEnable () {
		if (G.Sys.dataMaster.Coins == 0) {
			BestScoreImage.SetActive (false);
		}
		else {
			BestScoreImage.SetActive (true);
			BestScoreText.text = G.Sys.dataMaster.HighScoreText;
		}
	}
}
