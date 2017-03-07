using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ScoreEvent : EventArgs {

	public ScoreEvent(float _value)
	{
		Value = _value;
	}

	public float Value;
}
