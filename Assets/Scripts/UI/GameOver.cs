using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public Text CoinsGained;
	public GameObject NewRecord;
	public GameObject HUD;
	HUD hudScript;

	// Use this for initialization
	void Start () {
		hudScript = HUD.GetComponent <HUD>();
	}
	
	void OnEnable () {

		if (hudScript.Score > G.Sys.dataMaster.HighScore) {
			G.Sys.dataMaster.HighScore = hudScript.Score;
			NewRecord.SetActive (false);
		}
		else {
			NewRecord.SetActive (true);
		}

		G.Sys.dataMaster.Coins += hudScript.Coins;

		CoinsGained.text = hudScript.CoinText.text;

		HUD.SetActive (false);
	}
}
