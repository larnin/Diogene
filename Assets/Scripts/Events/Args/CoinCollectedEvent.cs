using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoinCollectedEvent : EventArgs {

	public CoinCollectedEvent(int _coinValue, float _multiplier)
	{
		Value = _coinValue;
        Multiplier = _multiplier;
	}

	public int Value;
    public float Multiplier;
}
