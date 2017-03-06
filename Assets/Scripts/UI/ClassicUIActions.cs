using UnityEngine;
using System.Collections;

public class ClassicUIActions : MonoBehaviour {

	public MenuState WhereGoing;
	public GameObject MainMenu;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
			MainMenu.SetActive (true);
			gameObject.SetActive (false);
		}
	}

	public void BackToMainScreen () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
		MainMenu.SetActive (true);
		gameObject.SetActive (false);
	}
}
