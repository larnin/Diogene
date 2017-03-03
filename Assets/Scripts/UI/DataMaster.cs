using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class DataMaster {

	public Save _save = new Save();

	public string CoinsText = "0";
	public int Coins {
		get {return _save.Coins;}
		set {
			_save.Coins = value;
			SaveData ();

			int _nbChiffre = CustomLog (_save.Coins);
			int _nbSection = _nbChiffre / 3;
			int _nbLastSection = _nbChiffre % 3;

			if (_nbLastSection == 0) {
				_nbSection++;
				_nbLastSection = 3;
			}

			int _buffer = (_save.Coins % (int)Mathf.Pow (10, 3 * _nbSection)) / (int)Mathf.Pow (10, 3 * (_nbSection - 1));
			string _text = "" + _buffer;
			for (int i = _nbSection - 1; i > 0; i--) {
				_buffer = (_save.Coins % (int)Mathf.Pow (10, 3 * i)) / (int)Mathf.Pow (10, 3 * (i - 1));
				if (_buffer == 0) {
					_text += ",000";
				}
				else {
					_text += "," + _buffer;
				}
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

	public int CustomLog(float entry) {
		int bufferOut;
		if (entry == 0) {
			bufferOut = 0;
		}
		else if (entry > 0) {
			bufferOut = Mathf.FloorToInt(Mathf.Log10(entry));
		}
		else {
			bufferOut = Mathf.FloorToInt (Mathf.Log10 (entry * (-1)));
		}

		return bufferOut;
	}

	public void SetEverything () {
		Coins = _save.Coins;
		HighScore = _save.HighScore;
	}
}
