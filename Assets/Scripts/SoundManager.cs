using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using System;
using System.IO;

public class SoundManager : MonoBehaviour {
	
	[Serializable]
	public struct SoundAndVolume {
		[SoundGroupAttribute] public string Sound;
		public float Volume;
	}
	public SoundAndVolume[] Sounds;
	Dictionary<string, float> _sounds =  new Dictionary<string, float>();

	SubscriberList _subscriberList = new SubscriberList();

	void Awake () {
		for (int i = 0; i < Sounds.Length; i++) {
			_sounds.Add (Sounds[i].Sound, Sounds[i].Volume);
		}
		_subscriberList.Add (new Event<PlaySoundEvent>.Subscriber (PlaySound));
		_subscriberList.Add (new Event<StopSoundEvent>.Subscriber (StopSound));
		_subscriberList.Subscribe ();
	}

	void PlaySound (PlaySoundEvent e) {
		MasterAudio.PlaySoundAndForget (e.Sound, _sounds [e.Sound]);
	}

	void StopSound (StopSoundEvent e) {
		MasterAudio.StopAllOfSound (e.Sound);
	}
}