using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlaySoundEvent : EventArgs
{
	public PlaySoundEvent(string _sound)
	{
		Sound = _sound;
	}

	public string Sound;
}
