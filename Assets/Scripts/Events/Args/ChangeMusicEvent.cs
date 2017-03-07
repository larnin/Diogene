using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChangeMusicEvent : EventArgs
{
	public ChangeMusicEvent(string _sound)
	{
		Sound = _sound;
	}

	public string Sound;
}
