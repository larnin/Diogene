using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AmplitudeManager
{
    public string AppKey = "b51822f3f9f96915e667f9b46bd92fb2";
    private Amplitude _amplitude;

    public AmplitudeManager()
    {
        _amplitude = Amplitude.Instance;
        _amplitude.logging = true;
        _amplitude.init(AppKey);
        

        /*if (!String.IsNullOrEmpty(SystemInfo.deviceUniqueIdentifier))
            _amplitude.setUserId(SystemInfo.deviceUniqueIdentifier);*/

        _amplitude.startSession();

        /*Dictionary<string, object> UserProperties = new Dictionary<string, object>();
        UserProperties.Add("Test", "Some string");
        UserProperties.Add("An other entry", 42);
        _amplitude.setUserProperties(UserProperties);*/
        _amplitude.logEvent("Test");
        _amplitude.endSession();
    }
}
