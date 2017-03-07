using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class DataMaster {

	public Save _save = new Save();

	public int ScreenSleepTime;

    SubscriberList _subscriberList = new SubscriberList();

    public DataMaster()
    {
        _subscriberList.Add(new Event<PlayerHaveJumped>.Subscriber(OnJump));
        _subscriberList.Subscribe();
    }

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
        set
        {
            _save.HolesCount = value;
            SaveData();
        }
    }
    public int RingCount
    {
        get { return _save.RingCount; }
        set
        {
            _save.RingCount = value;
            SaveData();
        }
    }
    public int TrapCount
    {
        get { return _save.TrapCount; }
        set
        {
            _save.TrapCount = value;
            SaveData();
        }
    }
    public int ArmCount
    {
        get { return _save.ArmCount; }
        set
        {
            _save.ArmCount = value;
            SaveData();
        }
    }
    public int JumpCount { get { return _save.JumpCount; } }
    void OnJump(PlayerHaveJumped e)
    {
        _save.JumpCount++;
        SaveData();
    }

    public bool PlayTuto
    {
        get { return _save.PlayTuto; }
        set
        {
			_save.PlayTuto = value;
            SaveData();
        }
    }

	public float MusicVolume
	{
		get {return _save.MusicVolume;}
		set {
			_save.MusicVolume = value;
			SaveData ();
			Event<ChangeVolumeEvent>.Broadcast(new ChangeVolumeEvent());
		}
	}

	public float SoundVolume
	{
		get {return _save.SoundVolume;}
		set {
			_save.SoundVolume = value;
			SaveData ();
			Event<ChangeVolumeEvent>.Broadcast(new ChangeVolumeEvent());
		}
	}

    void SaveData () {
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream saveFile = File.Create(Application.persistentDataPath + "/save.diogene");

		formatter.Serialize(saveFile, _save);

		saveFile.Close();
	}

	public void SetEverything () {
		Coins = _save.Coins;
		HighScore = _save.HighScore;
	}
}
