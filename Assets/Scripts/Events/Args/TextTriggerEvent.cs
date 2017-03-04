using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TextTriggerEvent : EventArgs
{
    public TextTriggerEvent(string _text, float _time)
    {
        text = _text;
        time = _time;
    }

    public string text;
    public float time;
}
