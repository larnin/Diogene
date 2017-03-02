using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoinCollectedEvent : EventArgs {

	public CoinCollectedEvent(int _coinValue)
	{
		Value = _coinValue;
	}

	public int Value;
}
