﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeToEndGame = 2;
    public GameObject mainMenu;
    public GameObject hud;
	public GameObject Continue;
    public GameObject playerPrefab;
    public GameObject rate;
    public int RunBetween2Rates = 10;
    public Vector3 playerStartLocation;
    public Text textTuto;
    public GameObject SupportTextTuto;
    public string StoreLink;

    SubscriberList _subscriberList = new SubscriberList();
    Coroutine _textTutoCoroutine;

    void Awake()
    {

        G.Sys.gameManager = this;

        _subscriberList.Add(new Event<PlayerKillEvent>.Subscriber(OnPlayerKill));
        _subscriberList.Add(new Event<TextTriggerEvent>.Subscriber(OnTextTrigger));
        _subscriberList.Subscribe();
    }

    void OnPlayerKill(PlayerKillEvent e)
    {
        StartCoroutine(WaitAndEndGameCoroutine());
        if(G.Sys.chunkSpawner.chunkCount() > 1)
            G.Sys.dataMaster.PlayTuto = false;
    }

    void OnApplicationQuit()
    {
        Event<QuitEvent>.Broadcast(new QuitEvent());
    }

    IEnumerator WaitAndEndGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndGame);
        Event<EndGameEvent>.Broadcast(new EndGameEvent());
		Continue.SetActive (true);
    }

	void Start()
    {
        hud.SetActive(false);
		Event<ResetEvent>.Broadcast (new ResetEvent ());
        Event<InitializeEvent>.Broadcast(new InitializeEvent(new Vector3(0, 0, 0)));
		Event<PlayerMovedEvent>.Broadcast (new PlayerMovedEvent (playerStartLocation, 0));
		Event<InstantMoveCameraEvent>.Broadcast (new InstantMoveCameraEvent ());
    }

    public void GoToStartMenu()
    {
		Event<ResetEvent>.Broadcast (new ResetEvent ());
        mainMenu.SetActive(true);
        hud.SetActive(false);
		Event<InitializeEvent>.Broadcast(new InitializeEvent(new Vector3(0, 0, 0)));
		Event<PlayerMovedEvent>.Broadcast (new PlayerMovedEvent (playerStartLocation, 0));
		Event<InstantMoveCameraEvent>.Broadcast (new InstantMoveCameraEvent ());
    }

    public void RestartGame()
    {
		Event<ResetEvent>.Broadcast (new ResetEvent ());
        mainMenu.SetActive(false);
        hud.SetActive(true);
		Event<InitializeEvent>.Broadcast(new InitializeEvent(new Vector3(0, 0, 0)));
		InstanciatePlayer ();
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        hud.SetActive(true);
		Event<ResetEvent>.Broadcast (new ResetEvent ());
		Event<InitializeEvent>.Broadcast(new InitializeEvent(new Vector3(0, 0, 0)));
		InstanciatePlayer ();
    }

    public void ShowRate()
    {
        rate.SetActive(true);
    }

    public void OpenStoreLink()
    {
        Application.OpenURL(StoreLink);
    }

    public void InstanciatePlayer()
    {
        Instantiate(playerPrefab);
        playerPrefab.transform.position = playerStartLocation;
    }

    void OnTextTrigger(TextTriggerEvent e)
    {
        SupportTextTuto.SetActive(true);
        textTuto.text = e.text;
        if(_textTutoCoroutine != null)
            StopCoroutine(_textTutoCoroutine);
        _textTutoCoroutine = StartCoroutine(TextRemoveCoroutine(e.time));
    }

    IEnumerator TextRemoveCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        SupportTextTuto.SetActive(false);
    }

    void OnApplicationFocus(bool value)
    {
        if (!value)
            Event<QuitEvent>.Broadcast(new QuitEvent());
    }

    void Update()
    {
        Event<FrameEvent>.Broadcast(new FrameEvent());
    }
}
