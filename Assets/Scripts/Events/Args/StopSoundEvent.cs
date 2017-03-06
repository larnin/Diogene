using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StopSoundEvent : EventArgs
{
	public StopSoundEvent(string _sound)
	{
		Sound = _sound;
	}

	public string Sound;
}
