using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PauseRingEvent : EventArgs
{
	public PauseRingEvent(bool state)
	{
		State = state;
	}

	public bool State = false;
}
