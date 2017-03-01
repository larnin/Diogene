using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerMovedEvent : EventArgs
{
    public PlayerMovedEvent(Vector3 _pos, int _direction)
    {
        pos = _pos;
        direction = _direction;
    }

    public Vector3 pos;
    public int direction;
}
