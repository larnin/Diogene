using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum AchievementBigType {OneRun, Global};
public enum AchievementSpecificType {
	RollDistance,
	CoinCollected,
	BigCoinCollected,
	JumpCount,
	DeathCount
}

public class ProgressAchievementEvent : EventArgs {

	public ProgressAchievementEvent(AchievementBigType _myType, AchievementSpecificType _myAchievement, float _myValue)
	{
		MyType = _myType;
		MyAchievement = _myAchievement;
		Value = _myValue;
	}

	public AchievementBigType MyType;
	public AchievementSpecificType MyAchievement;
	public float Value;
}
