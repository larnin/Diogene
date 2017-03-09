using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuCinematic : MonoBehaviour
{
    public GameObject player;
    public Animation PlayerAnimation;
    public GameObject oldScreen;
    public GameObject newScreen;

    bool _playing = true;

	void Start ()
    {
	
	}

	void Update ()
    {
        if (_playing && !PlayerAnimation.isPlaying)
        {
            player.SetActive(false);
            _playing = false;
            StartCoroutine(EndAnimation());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainScreen();
        }
	}

    public void BackToMainScreen()
    {
        Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.MAIN));
        SceneManager.LoadScene(1);
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(2);
        newScreen.transform.DOLocalMoveY(6.4f, 2);
        oldScreen.transform.DOLocalMoveY(-1663.6f, 2);
    }
}