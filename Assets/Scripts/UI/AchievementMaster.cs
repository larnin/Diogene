using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class AchievementMaster : MonoBehaviour {

	SubscriberList _subscriberList = new SubscriberList();

	int _runCoin = 0;
	int _runBigCoin = 0;
	float _runScore = 0;
	int _runJump = 0;

	public float WindowDuration;
	public GameObject Window;
	public Text TitleZone;
	public GameObject AchievementZone;

	float _windowTimer = 0;

	void Awake () {
		_subscriberList.Add(new Event<CoinCollectedEvent>.Subscriber(CoinCollected));
		_subscriberList.Add(new Event<GameOverEvent>.Subscriber(GameOver));
		_subscriberList.Add(new Event<ScoreEvent>.Subscriber(Scored));
		_subscriberList.Add (new Event<PlayerKillEvent>.Subscriber (Death));
		_subscriberList.Add (new Event<PlayerHaveJumped>.Subscriber (Jumped));
		_subscriberList.Add (new Event<AchievementSucessEvent>.Subscriber (AchievementSucess));
		_subscriberList.Subscribe();
	}

	void Start () {
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.CoinCollected, G.Sys.dataMaster.RunCoins, true));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.CoinCollected, G.Sys.dataMaster.Coins, true));

		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.BigCoinCollected, G.Sys.dataMaster.RunBigCoins, true));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.BigCoinCollected, G.Sys.dataMaster.BigCoins, true));

		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.RollDistance, G.Sys.dataMaster.HighScore, true));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.RollDistance, G.Sys.dataMaster.Distance, true));

		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.DeathCount, G.Sys.dataMaster.Death, true));

		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.JumpCount, G.Sys.dataMaster.RunJump, true));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.JumpCount, G.Sys.dataMaster.JumpCount, true));
	}

	void Update () {
		if (_windowTimer > 0) {
			_windowTimer -= Time.deltaTime;
			if (_windowTimer <= 0) {
				Window.SetActive (false);
			}
		}
	}

	void CoinCollected (CoinCollectedEvent e) {
		_runCoin += e.Value;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.CoinCollected, _runCoin, false));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.CoinCollected, G.Sys.dataMaster.Coins + _runCoin, false));
		if (e.Value >= 10) {
			_runBigCoin++;
			Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.BigCoinCollected, _runBigCoin, false));
			Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.BigCoinCollected, G.Sys.dataMaster.BigCoins + _runBigCoin, false));
		}
	}

	void Scored (ScoreEvent e) {
		_runScore = e.Value;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.RollDistance, _runScore, false));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.RollDistance, G.Sys.dataMaster.Distance + _runScore, false));
	}

	void Death (PlayerKillEvent e) {
		G.Sys.dataMaster.Death++;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.DeathCount, G.Sys.dataMaster.Death, false));
	}

	void Jumped (PlayerHaveJumped e) {
		_runJump++;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.JumpCount, _runJump, false));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.JumpCount, G.Sys.dataMaster.JumpCount, false));
	}

	void GameOver (GameOverEvent e) {
		G.Sys.dataMaster.BigCoins += _runBigCoin;
		G.Sys.dataMaster.Distance += _runScore;

		_runScore = 0;

		if (_runJump > G.Sys.dataMaster.RunJump) {
			G.Sys.dataMaster.RunJump = _runJump;
		}
		_runJump = 0;

		if (_runCoin > G.Sys.dataMaster.RunCoins) {
			G.Sys.dataMaster.RunCoins = _runCoin;
		}
		_runCoin = 0;

		if (_runBigCoin > G.Sys.dataMaster.RunBigCoins) {
			G.Sys.dataMaster.RunBigCoins = _runBigCoin;
		}
		_runBigCoin = 0;
	}

	void AchievementSucess (AchievementSucessEvent e) {
		Window.SetActive (true);
		TitleZone.text = e.Title;
		_windowTimer = WindowDuration;
	}

	public void ShowAchievement (bool state) {
		AchievementZone.SetActive (state);
		Event<UpdateAchievementUIEvent>.Broadcast(new UpdateAchievementUIEvent(state));
	}
}
