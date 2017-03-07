using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class TitleScreen : MonoBehaviour {

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
}
