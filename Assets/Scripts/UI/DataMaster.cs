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
				if ((i != 0) && (((_coinText.Length - i) % 3) == 0)) {
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

    public int HolesCount
    {
        get { return _save.HolesCount; }
        set { _save.HolesCount = value; }
    }
    public int RingCount
    {
        get { return _save.RingCount; }
        set { _save.RingCount = value; }
    }
    public int TrapCount
    {
        get { return _save.TrapCount; }
        set { _save.TrapCount = value; }
    }
    public int ArmCount
    {
        get { return _save.ArmCount; }
        set { _save.ArmCount = value; }
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
