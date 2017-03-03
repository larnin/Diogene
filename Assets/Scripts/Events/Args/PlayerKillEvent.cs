using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlayerKillEvent : EventArgs
{
    public PlayerKillEvent(float _speed)
    {
        speed = _speed;
    }
    public float speed;
}