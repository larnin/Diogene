using UnityEngine;
using System.Collections;

public enum PowerupType
{
    MAGNET = 0,
    FALL = 1,
    SHIELD = 2,
    MULTIPLIER = 3,
    DOUBLE_JUMP = 4,
    POWERUP_MAX = DOUBLE_JUMP
}

public class PowerUp : MonoBehaviour
{
    public PowerupType Type;
}
