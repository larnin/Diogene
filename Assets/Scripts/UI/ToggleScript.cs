using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour {

	public Toggle[] toggles;

	int buffer = 0;

	public void ToggleThem (Toggle ID) {

		if (buffer <= 0) {
			if (!ID.isOn) {
				buffer = 1;
				ID.isOn = true;
			}
			else {
				for (int i = 0; i < toggles.Length; i++) {
					if (toggles[i] != ID) {
						if (toggles[i].isOn) {
							buffer++;
							toggles [i].isOn = false;
						}
					}
				}
			}
		}
		else {
			buffer--;
		}
	}
}
