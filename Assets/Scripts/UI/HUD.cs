using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public float DistanceScale = 100;

	public Text ScoreText;
	public Text CoinText;

	[HideInInspector]
	public int Coins = 0;
	[HideInInspector]
	public float Score = 0;

	float startingY = 0;
	SubscriberList _subscriberList = new SubscriberList();

	void Start () {
		_subscriberList.Add(new Event<PlayerMovedEvent>.Subscriber(UpdateScore));
		_subscriberList.Add(new Event<CoinCollectedEvent>.Subscriber(UpdateCoin));
		_subscriberList.Subscribe ();

		ScoreText.text = "0";
		CoinText.text = "0";
	}

	void UpdateScore (PlayerMovedEvent e) {
		float _buffer = (e.pos.y - startingY) / DistanceScale;
		if (Mathf.Abs (_buffer) > Score) {
			Score = Mathf.Abs (_buffer);
			ScoreText.text = (Mathf.Floor (Score * 10) / 10) + "m";
		}
	}

	void UpdateCoin (CoinCollectedEvent e) {
		Coins += e.Value;

		int _nbChiffre = G.Sys.dataMaster.CustomLog (Coins);
		int _nbSection = _nbChiffre / 3;
		int _nbLastSection = _nbChiffre % 3;

		if (_nbLastSection == 0) {
			_nbSection++;
			_nbLastSection = 3;
		}

		int _buffer = (Coins % (int)Mathf.Pow (10, 3 * _nbSection)) / (int)Mathf.Pow (10, 3 * (_nbSection - 1));
		string _text = "" + _buffer;
		for (int i = _nbSection - 1; i > 0; i--) {
			_buffer = (Coins % (int)Mathf.Pow (10, 3 * i)) / (int)Mathf.Pow (10, 3 * (i - 1));
			if (_buffer == 0) {
				_text += ",000";
			}
			else {
				_text += "," + _buffer;
			}
		}

		CoinText.text = _text;
	}

	void OnEnable () {
		Coins = 0;
		Score = 0;

		ScoreText.text = "0";
		CoinText.text = "0";
	}
}
