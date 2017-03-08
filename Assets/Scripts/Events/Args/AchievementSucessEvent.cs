using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AchievementSucessEvent : EventArgs {

	public AchievementSucessEvent (string _title)
	{
		Title = _title;
	}

	public string Title;
}
