using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MoveRingEvent : EventArgs
{
    public MoveRingEvent(int _direction)
    {
        direction = _direction;
    }

    public int direction;
}