using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestEvent : EventArgs
{
    public TestEvent(string _text)
    {
        text = _text;
    }

    public string text;
}
