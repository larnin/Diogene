using UnityEngine;
using System.Collections;
using DarkTonic.MasterAudio;

public class TitleScreen : MonoBehaviour {

	[SoundGroupAttribute] public string MenuMusic;

	void Start () {
		Event<PlaySoundEvent>.Broadcast(new PlaySoundEvent(MenuMusic));
	}
}
