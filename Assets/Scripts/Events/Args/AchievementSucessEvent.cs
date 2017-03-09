using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AchievementSucessEvent : EventArgs {

	public AchievementSucessEvent (string _title, int _reward)
	{
		Title = _title;
		Reward = _reward;
	}

	public string Title;
	public int Reward;
}
