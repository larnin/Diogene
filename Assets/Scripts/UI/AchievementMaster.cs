using UnityEngine;
using System.Collections;

public class AchievementMaster : MonoBehaviour {

	SubscriberList _subscriberList = new SubscriberList();

	int _runCoin = 0;
	int _runBigCoin = 0;
	float _runScore = 0;
	int _runJump = 0;

	void Awake () {
		_subscriberList.Add(new Event<CoinCollectedEvent>.Subscriber(CoinCollected));
		_subscriberList.Add(new Event<GameOverEvent>.Subscriber(GameOver));
		_subscriberList.Add(new Event<ScoreEvent>.Subscriber(Scored));
		_subscriberList.Add (new Event<PlayerKillEvent>.Subscriber (Death));
		_subscriberList.Add (new Event<PlayerHaveJumped>.Subscriber (Jumped));
		_subscriberList.Subscribe();
	}
	
	void CoinCollected (CoinCollectedEvent e) {
		_runCoin += e.Value;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.CoinCollected, _runCoin));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.CoinCollected, G.Sys.dataMaster.Coins + _runCoin));
		if (e.Value >= 10) {
			_runBigCoin++;
			Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.CoinCollected, _runBigCoin));
			Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.CoinCollected, G.Sys.dataMaster.BigCoins + _runBigCoin));
		}
	}

	void Scored (ScoreEvent e) {
		_runScore = e.Value;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.RollDistance, _runScore));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.RollDistance, G.Sys.dataMaster.Distance + _runScore));
	}

	void Death (PlayerKillEvent e) {
		G.Sys.dataMaster.Death++;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.DeathCount, G.Sys.dataMaster.Death));
	}

	void Jumped (PlayerHaveJumped e) {
		_runJump++;
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.OneRun, AchievementSpecificType.CoinCollected, _runJump));
		Event<ProgressAchievementEvent>.Broadcast(new ProgressAchievementEvent(AchievementBigType.Global, AchievementSpecificType.CoinCollected, G.Sys.dataMaster.JumpCount));
	}

	void GameOver (GameOverEvent e) {
		G.Sys.dataMaster.BigCoins += _runBigCoin;
		G.Sys.dataMaster.Distance += _runScore;

		_runJump = 0;
		_runScore = 0;
		_runCoin = 0;
		_runBigCoin = 0;
	}
}
