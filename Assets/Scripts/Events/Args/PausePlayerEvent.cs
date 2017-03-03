using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PausePlayerEvent : EventArgs
{
	public PausePlayerEvent(bool state)
	{
		State = state;
	}

	public bool State = false;
}
