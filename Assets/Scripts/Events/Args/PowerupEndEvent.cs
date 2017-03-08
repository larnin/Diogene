using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PowerupEndEvent : EventArgs
{
    public PowerupEndEvent(PowerupType _type)
    {
        type = _type;
    }

    public PowerupType type;
}
