using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using UnityEngine;

public enum MenuState
{
    TITLE,
	MAIN,
    PLAY,
    GAMEOVER,
	PAUSE,
	OPTIONS,
	CREDITS
}

public class ChangeMenuEvent : EventArgs
{
    public ChangeMenuEvent(MenuState _state)
    {
        state = _state;
    }

	public MenuState state;
}
