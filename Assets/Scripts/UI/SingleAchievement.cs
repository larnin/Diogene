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
						//success
					}
				}
			}
		}
	}

	void UpdateAchievementUI (UpdateAchievementUIEvent e) {
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
				_textBuffer = "Roll " + Value.ToString () + " meters in one run !";
			}
			else if (MyAchievement == AchievementSpecificType.CoinCollected) {
				_textBuffer = "Collect " + Value.ToString () + " coins in one run !";
			}
			else if (MyAchievement == AchievementSpecificType.BigCoinCollected) {
				_textBuffer = "Collect " + Value.ToString () + " big coins in one run !";
			}
			else if (MyAchievement == AchievementSpecificType.JumpCount) {
				_textBuffer = "Jump " + Value.ToString () + " times in one run !";
			}
		}
		else {
			if (MyAchievement == AchievementSpecificType.RollDistance) {
				_textBuffer = "Roll " + Value.ToString () + " meters.";
				if (!_done) {
						_textBuffer += "(Current: " + _currentValue.ToString () + ")";
				}
			}
			else if (MyAchievement == AchievementSpecificType.CoinCollected) {
				_textBuffer = "Collect " + Value.ToString () + " coins.";
				if (!_done) {
					_textBuffer += "(Current: " + _currentValue.ToString () + ")";
				}
			}
			else if (MyAchievement == AchievementSpecificType.BigCoinCollected) {
				_textBuffer = "Collect " + Value.ToString () + " big coins.";
				if (!_done) {
					_textBuffer += "(Current: " + _currentValue.ToString () + ")";
				}
			}
			else if (MyAchievement == AchievementSpecificType.JumpCount) {
				_textBuffer = "Jump " + Value.ToString () + " times.";
				if (!_done) {
					_textBuffer += "(Current: " + _currentValue.ToString () + ")";
				}
			}
			else if (MyAchievement == AchievementSpecificType.DeathCount) {
				_textBuffer = "Die " + Value.ToString () + " times.";
				if (!_done) {
					_textBuffer += "(Current: " + _currentValue.ToString () + ")";
				}
			}
		}

		RewardZone.text = _textBuffer;
	}
}
