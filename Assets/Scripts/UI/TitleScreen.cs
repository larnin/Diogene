using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class TitleScreen : MonoBehaviour {

	void Awake () {

		G.Sys.dataMaster.ScreenSleepTime = Screen.sleepTimeout;
	}
}
