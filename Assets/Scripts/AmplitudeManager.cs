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

    public float _distanceMax = 0;
    public MenuState _lastMenu = MenuState.TITLE;
    public float _lastChangeMenuTime = 0;
    public float _sumRunTime = 0;
    public float _sumCoin = 0;
    public float _runCount = 0;

    public AmplitudeManager()
    {
        _subscriberList.Add(new Event<GameOverEvent>.Subscriber(OnGameOver));
        _subscriberList.Add(new Event<InitializeEvent>.Subscriber(OnStartRun));
        _subscriberList.Add(new Event<PlayerHaveJumped>.Subscriber(OnJump));
        _subscriberList.Add(new Event<QuitEvent>.Subscriber(OnQuit));
        _subscriberList.Add(new Event<ChangeMenuEvent>.Subscriber(OnMenuChange));
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

        _sumCoin += e.RunCoin;
        _runCount++;
        _sumRunTime += time;
        if (e.RunScore > _distanceMax)
            _distanceMax = e.RunScore;

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

    void OnQuit(QuitEvent e)
    {
        Dictionary<string, object> UserProperties = new Dictionary<string, object>();
        UserProperties.Add("SDistanceMax", _distanceMax);
        UserProperties.Add("SLastWindow", _lastMenu);
        UserProperties.Add("SCoins", _sumCoin);
        UserProperties.Add("STSession", Time.time);
        UserProperties.Add("SRun", _runCount);
        UserProperties.Add("STMenus", Time.time - _sumRunTime);
        UserProperties.Add("STRuns", _sumRunTime);
        _amplitude.logEvent("EndSession", UserProperties);

        _distanceMax = 0;
        _sumRunTime = 0;
        _sumCoin = 0;
        _runCount = 0;
    }

    void OnMenuChange(ChangeMenuEvent e)
    {
        float currentTime = Time.time - _lastChangeMenuTime;
        _lastChangeMenuTime = Time.time;

        if (_lastMenu != MenuState.PLAY)
        {
            Dictionary<string, object> UserProperties = new Dictionary<string, object>();
            UserProperties.Add("MOpened", _lastMenu.ToString());
            UserProperties.Add("MTMain", currentTime);
            _amplitude.logEvent("Menu", UserProperties);
        }

        _lastMenu = e.state;
    }
    
    void OnJump(PlayerHaveJumped e)
    {
        _jumpCount++;
    }
}
