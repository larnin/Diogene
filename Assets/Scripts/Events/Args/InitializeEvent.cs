using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InitializeEvent : EventArgs
{
    public InitializeEvent(Vector3 _pos)
    {
        pos = _pos;
    }

    public Vector3 pos;
}