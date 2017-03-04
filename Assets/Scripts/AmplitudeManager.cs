using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AmplitudeManager
{
    public string AppKey = "b51822f3f9f96915e667f9b46bd92fb2";
    private Amplitude _amplitude;
    private SubscriberList _subscriberList = new SubscriberList();
    private float _maxSpeed = 0;
    public float _startTime = 0;
    public float _jumpCount = 0;

    public AmplitudeManager()
    {
        _subscriberList.Add(new Event<GameOverEvent>.Subscriber(OnGameOver));
        _subscriberList.Add(new Event<InitializeEvent>.Subscriber(OnStartRun));

        _subscriberList.Subscribe();

        _amplitude = Amplitude.Instance;
        _amplitude.logging = true;
        _amplitude.init(AppKey);
        

        /*if (!String.IsNullOrEmpty(SystemInfo.deviceUniqueIdentifier))
            _amplitude.setUserId(SystemInfo.deviceUniqueIdentifier);*/

        //_amplitude.startSession();

        /*Dictionary<string, object> UserProperties = new Dictionary<string, object>();
        UserProperties.Add("Test", "Some string");
        UserProperties.Add("An other entry", 42);
        _amplitude.setUserProperties(UserProperties);*/
        //_amplitude.logEvent("Test");
        //_amplitude.endSession();
    }

    void OnStartRun(InitializeEvent e)
    {
        _maxSpeed = 0;
        _startTime = Time.time;
        _jumpCount = 0;
    }

    void OnKillEvent(PlayerKillEvent e)
    {
        _maxSpeed = e.speed;
    }

    void OnGameOver(GameOverEvent e)
    {
        float time = Time.time - _startTime;
        var chunkDatas = G.Sys.chunkSpawner.allDatas();

        Dictionary<string, object> UserProperties = new Dictionary<string, object>();
        UserProperties.Add("RJump", _jumpCount);
        UserProperties.Add("Rhole", chunkDatas.holesCount);
        UserProperties.Add("RDistanceMax", e.RunScore);
        UserProperties.Add("RChunkDeath", G.Sys.chunkSpawner.currentChunkID());
        UserProperties.Add("RCoins", e.RunCoin);
        UserProperties.Add("RCheat", e.RunCoin / e.RunScore);
        UserProperties.Add("RRing", chunkDatas.ringCount);
        UserProperties.Add("RTRun", time);
        UserProperties.Add("RArms", chunkDatas.armCount);
        _amplitude.logEvent("EndRun", UserProperties);
    }
    
    void OnJump(PlayerJumpEvent e)
    {
        _jumpCount++;
    }
}
