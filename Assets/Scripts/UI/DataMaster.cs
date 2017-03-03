using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class DataMaster {

	public Save _save = new Save();

	public int ScreenSleepTime;

	public string CoinsText = "0";
	public int Coins {
		get {return _save.Coins;}
		set {
			_save.Coins = value;
			SaveData ();

			string _coinText = Coins.ToString ();
			string _text = "";
			for (int i = 0; i < _coinText.Length; i++) {
				if (((_coinText.Length - i) % 3) == 0) {
					_text += ",";
				}
				_text += _coinText[i];
			}

			CoinsText = _text;
		}
	}

	public string HighScoreText = "0m";
	public float HighScore {
		get {return _save.HighScore;}
		set {
			_save.HighScore = value;
			SaveData ();

			HighScoreText = (Mathf.Floor (HighScore * 10) / 10) + "m";
		}
	}

	void SaveData () {
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream saveFile = File.Create("Saves/save.diogene");

		formatter.Serialize(saveFile, _save);

		saveFile.Close();
	}

	public void SetEverything () {
		Coins = _save.Coins;
		HighScore = _save.HighScore;
	}
}
