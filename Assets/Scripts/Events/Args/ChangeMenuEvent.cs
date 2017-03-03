using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum MenuState
{
    Main,
    Game,
    End
}

public class ChangeMenuEvent : EventArgs
{
    public ChangeMenuEvent(MenuState _state)
    {
        state = _state;
    }

    public MenuState state;
}
