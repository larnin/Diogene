using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameOverEvent : EventArgs {

	public GameOverEvent(float _runScore, int _runCoin)
	{
		RunScore = _runScore;
		RunCoin = _runCoin;
	}

	public float RunScore;
	public int RunCoin;
}
