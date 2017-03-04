using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeToEndGame = 2;
    public GameObject mainMenu;
    public GameObject hud;
	public GameObject Continue;
    public GameObject playerPrefab;
    public Vector3 playerStartLocation;

    private static bool _instanciated = false;
    SubscriberList _subscriberList = new SubscriberList();

    void Awake()
    {
        DontDestroyOnLoad(this);

		if (!_instanciated)
			_instanciated = true;
		else {
			Destroy (gameObject);
			return;
		}

        G.Sys.gameManager = this;

        _subscriberList.Add(new Event<PlayerKillEvent>.Subscriber(OnPlayerKill));
        _subscriberList.Subscribe();
    }

    void OnPlayerKill(PlayerKillEvent e)
    {
        StartCoroutine(WaitAndEndGameCoroutine());
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
        Event<InitializeEvent>.Broadcast(new InitializeEvent(new Vector3(0, 0, 0)));
		Event<PlayerMovedEvent>.Broadcast (new PlayerMovedEvent (playerStartLocation, 0));
		Event<InstantMoveCameraEvent>.Broadcast (new InstantMoveCameraEvent ());
        //InstanciatePlayer();
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
		InstanciatePlayer ();
    }

    public void InstanciatePlayer()
    {
        Instantiate(playerPrefab);
        playerPrefab.transform.position = playerStartLocation;
    }
}
