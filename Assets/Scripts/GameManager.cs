using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeToEndGame = 2;
    public GameObject mainMenu;
    public GameObject hud;
    public GameObject playerPrefab;
    public Vector3 playerStartLocation;

    private static bool _instanciated = false;
    SubscriberList _subscriberList = new SubscriberList();

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (!_instanciated)
            _instanciated = true;
        else Destroy(this);

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
    }

	void Start()
    {
        mainMenu.SetActive(true);
        hud.SetActive(false);
        Event<InitializeEvent>.Broadcast(new InitializeEvent(new Vector3(0, 0, 0)));
        //InstanciatePlayer();
    }

    public void GoToStartMenu()
    {
        SceneManager.LoadScene(0);
        mainMenu.SetActive(true);
        hud.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        mainMenu.SetActive(false);
        hud.SetActive(true);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        hud.SetActive(true);
    }

    public void InstanciatePlayer()
    {
        Instantiate(playerPrefab);
        playerPrefab.transform.position = playerStartLocation;
    }
}
