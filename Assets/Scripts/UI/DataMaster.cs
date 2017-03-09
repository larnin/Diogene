using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class DataMaster {

    Save _save = new Save();

	public int ScreenSleepTime;

    SubscriberList _subscriberList = new SubscriberList();

    static int _version = 1;

    public DataMaster()
    {
        _subscriberList.Add(new Event<PlayerHaveJumped>.Subscriber(OnJump));
        _subscriberList.Subscribe();

        Load();
    }

    void Load()
    {

        /*
		_save = new Save ();
		SaveData ();
		*/
        bool failled = false;
        try
        {
            if (!File.Exists(Application.persistentDataPath + "/save.diogene"))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream saveFile = File.Create(Application.persistentDataPath + "/save.diogene");

                _save = new Save();

                formatter.Serialize(saveFile, new Save());

                saveFile.Close();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream saveFile = File.Open(Application.persistentDataPath + "/save.diogene", FileMode.Open);

                _save = (Save)formatter.Deserialize(saveFile);

                saveFile.Close();

                SetEverything();
            }
        }
        catch(Exception e)
        {
            Debug.LogWarning("Can't load the file !");
            _save = new Save();
            failled = true;
        }

        if(_save.Version != _version || _save.PowerupLevel == null || failled)
        {
            int Coins = _save.Coins;
            float HighScore = _save.HighScore;
            bool tutoPlayed = _save.PlayTuto;
            _save = new Save();
            _save.Version = _version;
            _save.HighScore = HighScore;
            _save.Coins = Coins;
            _save.PlayTuto = tutoPlayed;
            SaveData();
        }

        _save.PowerupLevel = new int[(int)PowerupType.POWERUP_MAX + 1] { 1, 1, 1, 1, 1 };
    }

    public void Reset()
    {
        _save = new Save();
        SaveData();
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

	public int BigCoins
	{
		get { return _save.BigCoins; }
		set
		{
			_save.BigCoins = value;
			SaveData();
		}
	}

	public int RunBigCoins
	{
		get { return _save.RunBigCoins; }
		set
		{
			_save.RunBigCoins = value;
			SaveData();
		}
	}

	public int RunCoins
	{
		get { return _save.RunCoins; }
		set
		{
			_save.RunCoins = value;
			SaveData();
		}
	}

	public int GlobalCoin
	{
		get { return _save.GlobalCoin; }
		set
		{
			_save.GlobalCoin = value;
			SaveData();
		}
	}

	public float Distance
	{
		get { return _save.Distance; }
		set
		{
			_save.Distance = value;
			SaveData();
		}
	}

	public int Death
	{
		get { return _save.Death; }
		set
		{
			_save.Death = value;
			SaveData();
		}
	}

	public int RunJump
	{
		get { return _save.RunJump; }
		set
		{
			_save.RunJump = value;
			SaveData();
		}
	}

    public int PowerupLevel(PowerupType type)
    {
        return _save.PowerupLevel[(int)type];
    }

    public void SetPowerupLevel(int value, PowerupType type)
    {
        _save.PowerupLevel[(int)type] = value;
        SaveData();
    }

	public bool CosmeticsLevel(CosmeticsType type)
	{
		return _save.CosmeticsLevel[(int)type];
	}

	public void SetCosmeticsLevel(bool value, CosmeticsType type)
	{
		_save.CosmeticsLevel[(int)type] = value;
		SaveData();
	}

	public CosmeticsType EquippedCosmetic
	{
		get { return _save.EquippedCosmetic; }
		set
		{
			_save.EquippedCosmetic = value;
			SaveData();
		}
	}

    void SaveData () {
        try
        {

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream saveFile = File.Create(Application.persistentDataPath + "/save.diogene");

		formatter.Serialize(saveFile, _save);

		saveFile.Close();
        }
        catch(Exception e)
        {
            Debug.LogWarning("Can't save the file !");
        }
	}

	public void SetEverything () {
		Coins = _save.Coins;
		HighScore = _save.HighScore;
	}
}
