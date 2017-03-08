using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PowerupFlashEvent : EventArgs
{
    public PowerupFlashEvent(PowerupType _type)
    {
        type = _type;
    }

    public PowerupType type;
}