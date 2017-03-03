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

		string _coinText = Coins.ToString ();
		string _text = "";
		for (int i = 0; i < _coinText.Length; i++) {
			if ((i != 0) && (((_coinText.Length - i) % 3) == 0)) {
				_text += ",";
			}
			_text += _coinText[i];
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
