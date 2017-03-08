using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UpdateAchievementUIEvent : EventArgs
{
	public UpdateAchievementUIEvent (bool _state) {
		State = _state;
	}
	public bool State;
}
