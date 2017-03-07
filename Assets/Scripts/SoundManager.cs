using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using System;
using System.IO;

public class SoundManager : MonoBehaviour {

	public PlaylistController MyPlaylist;

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
		_subscriberList.Add (new Event<ChangeMusicEvent>.Subscriber (PlayMusic));
		_subscriberList.Add (new Event<ChangeVolumeEvent>.Subscriber (ChangeVolume));
		_subscriberList.Subscribe ();
	}

	void Start () {
		ChangeVolume (new ChangeVolumeEvent());
	}

	void PlaySound (PlaySoundEvent e) {
		MasterAudio.PlaySoundAndForget (e.Sound, _sounds [e.Sound]);
	}

	void PlayMusic (ChangeMusicEvent e) {
		MyPlaylist.ChangePlaylist (e.Sound, true);
	}

	void ChangeVolume (ChangeVolumeEvent e) {
		MasterAudio.MasterVolumeLevel = G.Sys.dataMaster.SoundVolume;
		MasterAudio.PlaylistMasterVolume = G.Sys.dataMaster.MusicVolume;
	}
}