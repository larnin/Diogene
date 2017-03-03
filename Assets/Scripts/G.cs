using UnityEngine;
using System.Collections;

public sealed class G
{
	private static volatile G instance;
    private GameManager _manager;
    private System.Random _random = new System.Random();
	private DataMaster _dataMaster = new DataMaster();
    private AmplitudeManager _amplitudeManager = new AmplitudeManager();

    public static G Sys
    {
        get
        {
            if (G.instance == null)
                G.instance = new G();
            return G.instance;
        }
    }

	public DataMaster dataMaster
	{
		get {return _dataMaster;}
	}

    public GameManager gameManager
    {
        get { return _manager; }
        set
        {
            if (_manager != null)
                Debug.Log("2 GameManagers Instanciated!");
            _manager = value;
        }
    }

    public System.Random random
    {
        get { return _random; }
    }

    public AmplitudeManager amplitudeManager
    {
        get { return _amplitudeManager; }
    }
}
