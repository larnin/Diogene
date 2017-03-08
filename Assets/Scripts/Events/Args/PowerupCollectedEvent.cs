using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PowerupCollectedEvent : EventArgs
{
    public PowerupCollectedEvent(PowerupType _type)
    {
        type = _type;
    }

    public PowerupType type;
}
