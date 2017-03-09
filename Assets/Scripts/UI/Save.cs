using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Save {

    public int Version = 0;
	public int Coins = 0;
	public float HighScore = 0;
    public int HolesCount = 0;
    public int RingCount = 0;
    public int TrapCount = 0;
    public int ArmCount = 0;
    public int JumpCount = 0;
    public bool PlayTuto = true;
	public float MusicVolume = 1;
	public float SoundVolume = 1;
	public int BigCoins = 0;
	public float Distance = 0;
	public int Death = 0;
	public int RunCoins = 0;
	public int GlobalCoin = 0;
	public int RunBigCoins = 0;
	public int RunJump = 0;
    public int[] PowerupLevel = new int[(int)PowerupType.POWERUP_MAX + 1];
	public bool[] CosmeticsLevel = new bool[(int)CosmeticsType.COSMETICS_MAX + 1];
	public CosmeticsType EquippedCosmetic = CosmeticsType.DEFAULT;
	public int UnlockedAchievements = 0;
	public int RunPowerUp = 0;
	public int GlobalPowerUp = 0;
}
