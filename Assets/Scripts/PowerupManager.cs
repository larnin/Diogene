using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour
{
    public List<float> magnetTimes;
    public List<float> fallTimes;
    public List<float> shieldTimes;
    public List<float> multiplierTimes;
    public List<float> doubleJumpTimes;

    public float FlashTime;
    public float ShieldTimeAfterPowerUp;

    public float MagnetRadius;
    public float MagnetPower;
    public float MaxFallSpeed;

    List<float> _times = new List<float>{0, 0, 0, 0, 0};
    SubscriberList _subscriberList = new SubscriberList();

    void Awake()
    {
        G.Sys.powerupManager = this;

        _subscriberList.Add(new Event<PowerupCollectedEvent>.Subscriber(OnPowerupCollect));
        _subscriberList.Subscribe();
    }

	void Update ()
    {
	    for(int i = 0; i < _times.Count; i++)
        {
            float old = _times[i];
            _times[i] -= Time.deltaTime;
            if (old > FlashTime && _times[i] <= FlashTime)
                Event<PowerupFlashEvent>.Broadcast(new PowerupFlashEvent((PowerupType)i));
            if(old > 0 && _times[i] <= 0)
                Event<PowerupEndEvent>.Broadcast(new PowerupEndEvent((PowerupType)i));
        }
	}

    public bool IsEnabled(PowerupType type)
    {
        return _times[(int)type] > 0;
    }

    public bool IsOnShieldTime()
    {
        if (_times[(int)PowerupType.FALL] < 0 && _times[(int)PowerupType.FALL] > -ShieldTimeAfterPowerUp)
            return true;
        return false;
    }

    void OnPowerupCollect(PowerupCollectedEvent e)
    {
        int level = G.Sys.dataMaster.PowerupLevel(e.type);
        switch(e.type)
        {
            case PowerupType.MAGNET:
                _times[(int)e.type] = magnetTimes[level];
                break;
            case PowerupType.FALL:
                _times[(int)e.type] = fallTimes[level];
                break;
            case PowerupType.SHIELD:
                _times[(int)e.type] = shieldTimes[level];
                break;
            case PowerupType.MULTIPLIER:
                _times[(int)e.type] = multiplierTimes[level];
                break;
            case PowerupType.DOUBLE_JUMP:
            default:
                _times[(int)e.type] = doubleJumpTimes[level];
                break;
        }
    }

    void OnReset(ResetEvent e)
    {
        for (int i = 0; i < _times.Count; i++)
            _times[i] = 0;
    }
}