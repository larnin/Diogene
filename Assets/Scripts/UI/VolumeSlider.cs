using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class VolumeSlider : MonoBehaviour {

	public enum VolumeType {Music, Sound};
	public VolumeType Type;
	Slider _mySlider;

	void Start () {
		_mySlider = gameObject.GetComponent <Slider> ();
		float buffer;
		if (Type == VolumeType.Music) {
			buffer = G.Sys.dataMaster.MusicVolume;
		}
		else {
			buffer = G.Sys.dataMaster.SoundVolume;
		}
		_mySlider.value = buffer;
	}

	public void UpdateVolume () {
		if (Type == VolumeType.Music) {
			G.Sys.dataMaster.MusicVolume = _mySlider.value;
		}
		else {
			G.Sys.dataMaster.SoundVolume = _mySlider.value;
		}
	}
}
