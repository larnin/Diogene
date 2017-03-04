using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	/*
	[SoundGroupAttribute] public string HackingSound;
	public float HackingSoundVolume = 1f;
	[SoundGroupAttribute] public string EnterSound;
	public float EnterSoundVolume = 1f;
	[SoundGroupAttribute] public string JumpSound;
	public float JumpSoundVolume = 1f;
	[SoundGroupAttribute] public string LandingSound;
	public float LandingSoundVolume = 1f;
	[SoundGroupAttribute] public string WalkSound;
	public float WalkSoundVolume = 1f;
	[SoundGroupAttribute] public string RunSound;
	public float RunSoundVolume = 1f;
	*/
	void Start () {
	}

	public void PlayHackSound () {
		//MasterAudio.PlaySound3DFollowTransformAndForget (HackingSound, transform, HackingSoundVolume);
	}

	public void StopHackSound () {
		//MasterAudio.StopAllOfSound(HackingSound);
	}
}
