using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public Toggle[] toggles;

	void OnEnable () {
		if (G.Sys.dataMaster.PlayTuto) {
			if (!toggles[0].isOn) {
				toggles [0].isOn = true;
			}
		}
		else {
			if (!toggles[1].isOn) {
				toggles [1].isOn = true;
			}
		}
	}

	public void ChangeValue (Toggle ID) {
		if (ID.isOn) {
			if (ID == toggles [0]) {
				G.Sys.dataMaster.PlayTuto = true;
			}
			else {
				G.Sys.dataMaster.PlayTuto = false;
			}
		}
	}
}
