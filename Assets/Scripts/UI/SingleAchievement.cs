using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SingleAchievement : MonoBehaviour {

	public AchievementBigType MyType;
	public AchievementSpecificType MyAchievement;
	public float Value;
	public string Title;
	public int CoinValue;

	float _currentValue;
	bool _done;

	[Header("Don't Touch This")]
	public Text TitleZone;
	public Text DescriptionZone;
	public Text RewardZone;
	public GameObject Locked;
	public GameObject Unlocked;
	public GameObject GlobalZone;

	SubscriberList _subscriberList = new SubscriberList();

	void Awake () {
		_subscriberList.Add(new Event<UpdateAchievementUIEvent>.Subscriber(UpdateAchievementUI));
		_subscriberList.Add(new Event<ProgressAchievementEvent>.Subscriber(UpdateAchievementProgress));
		_subscriberList.Subscribe();
	}

	void UpdateAchievementProgress (ProgressAchievementEvent e) {
		if (!_done) {
			if (e.MyType == MyType) {
				if (e.MyAchievement == MyAchievement) {
					_currentValue = e.Value;
					if (_currentValue >= Value) {
						_done = true;
						if (!e.Start) {
							Event<AchievementSucessEvent>.Broadcast(new AchievementSucessEvent(Title));
							G.Sys.dataMaster.Coins += CoinValue;
						}
					}
				}
			}
		}
	}

	void UpdateAchievementUI (UpdateAchievementUIEvent e) {

		if (e.State) {

			GlobalZone.SetActive (true);

			if (_done) {
				Locked.SetActive (false);
				Unlocked.SetActive (true);
			}
			else {
				Locked.SetActive (true);
				Unlocked.SetActive (false);
			}
			
			TitleZone.text = Title;
			RewardZone.text = CoinValue.ToString ();
			
			string _textBuffer = string.Empty;
			
			if (MyType == AchievementBigType.OneRun) {
				if (MyAchievement == AchievementSpecificType.RollDistance) {
					_textBuffer = "ROLL " + Value.ToString () + " METERS IN ONE RUN !";
				}
				else if (MyAchievement == AchievementSpecificType.CoinCollected) {
					_textBuffer = "COLLECT " + Value.ToString () + " COINS IN ONE RUN !";
				}
				else if (MyAchievement == AchievementSpecificType.BigCoinCollected) {
					_textBuffer = "COLLECT " + Value.ToString () + " BIG COINS IN ONE RUN !";
				}
				else if (MyAchievement == AchievementSpecificType.JumpCount) {
					_textBuffer = "JUMP " + Value.ToString () + " TIMES IN ONE RUN !";
				}
			}
			else {
				if (MyAchievement == AchievementSpecificType.RollDistance) {
					_textBuffer = "ROLL " + Value.ToString () + " METERS.";
					if (!_done) {
						_textBuffer += "(CURRENT: " + _currentValue.ToString () + ")";
					}
				}
				else if (MyAchievement == AchievementSpecificType.CoinCollected) {
					_textBuffer = "COLLECT " + Value.ToString () + " COINS.";
					if (!_done) {
						_textBuffer += "(CURRENT: " + _currentValue.ToString () + ")";
					}
				}
				else if (MyAchievement == AchievementSpecificType.BigCoinCollected) {
					_textBuffer = "COLLECT " + Value.ToString () + " BIG COINS.";
					if (!_done) {
						_textBuffer += "(CURRENT: " + _currentValue.ToString () + ")";
					}
				}
				else if (MyAchievement == AchievementSpecificType.JumpCount) {
					_textBuffer = "JUMP " + Value.ToString () + " TIMES.";
					if (!_done) {
						_textBuffer += "(CURRENT: " + _currentValue.ToString () + ")";
					}
				}
				else if (MyAchievement == AchievementSpecificType.DeathCount) {
					_textBuffer = "DIE " + Value.ToString () + " TIMES.";
					if (!_done) {
						_textBuffer += "(CURRENT: " + _currentValue.ToString () + ")";
					}
				}
			}
			
			DescriptionZone.text = _textBuffer;
		}
		else {
			Locked.SetActive (false);
			Unlocked.SetActive (false);
			GlobalZone.SetActive (false);
		}

	}
}
