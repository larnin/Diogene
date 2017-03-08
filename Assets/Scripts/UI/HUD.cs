using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public float DistanceScale = 100;

	public GameObject Pause;
	public Text ScoreText;
	public Text CoinText;
    public Sprite RingArrowHiden;
    public Sprite RingArrowClicked;
    public float ShowArrowTime;
    public Image LeftArrowImage;
    public Image RightArrowImage;

	[HideInInspector]
	public int Coins = 0;
	[HideInInspector]
	public float Score = 0;

	float startingY = 0;
	SubscriberList _subscriberList = new SubscriberList();

	[HideInInspector]
	public bool CanPause = true;

    Coroutine _ringCoroutine;

	void Start () {
		_subscriberList.Add(new Event<PlayerMovedEvent>.Subscriber(UpdateScore));
		_subscriberList.Add(new Event<CoinCollectedEvent>.Subscriber(UpdateCoin));
		_subscriberList.Add(new Event<PlayerKillEvent>.Subscriber(CannotPause));
        _subscriberList.Add(new Event<MoveRingEvent>.Subscriber(OnRingClick));
		_subscriberList.Subscribe ();

		ScoreText.text = "0";
		CoinText.text = "0";
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
		{
			PauseGame ();
		}
	}

	void UpdateScore (PlayerMovedEvent e) {
		float _buffer = (e.pos.y - startingY) / DistanceScale;
		if (Mathf.Abs (_buffer) > Score) {
			Score = Mathf.Abs (_buffer);
			ScoreText.text = (Mathf.Floor (Score * 10) / 10) + "m";
		}
		Event<ScoreEvent>.Broadcast (new ScoreEvent (Mathf.Abs (_buffer)));
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

    void OnRingClick(MoveRingEvent e)
    {
        if(e.direction > 0)
        {
            LeftArrowImage.sprite = RingArrowClicked;
            RightArrowImage.sprite = RingArrowHiden;
        }
        else
        {
            RightArrowImage.sprite = RingArrowClicked;
            LeftArrowImage.sprite = RingArrowHiden;
        }
        if (_ringCoroutine != null)
            StopCoroutine(_ringCoroutine);
        StartCoroutine(OffRingCoroutine());
    }

    IEnumerator OffRingCoroutine()
    {
        yield return new WaitForSeconds(ShowArrowTime);
        LeftArrowImage.sprite = RingArrowHiden;
        RightArrowImage.sprite = RingArrowHiden;
    }

	void OnEnable () {
		Coins = 0;
		Score = 0;

		ScoreText.text = "0";
		CoinText.text = "0";

		CanPause = true;
	}

	public void PauseGame () {
		if (CanPause) {
			Event<PauseRingEvent>.Broadcast(new PauseRingEvent(true));
			Event<PausePlayerEvent>.Broadcast(new PausePlayerEvent(true));
			Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.PAUSE));
			Pause.SetActive (true);
		}
	}

	void CannotPause (PlayerKillEvent e) {
		CanPause = false;
	}
}
