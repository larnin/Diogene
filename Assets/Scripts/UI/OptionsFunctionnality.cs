using UnityEngine;
using System.Collections;

public class OptionsFunctionnality : MonoBehaviour {

	public MenuState WhereGoing;
	public GameObject MainMenu;
	public GameObject Credits;
	public GameObject Window;

	public void ResetData () {
        G.Sys.dataMaster.Reset();
		Event<RefreshEvent>.Broadcast(new RefreshEvent());
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
		Window.SetActive (false);
		MainMenu.SetActive (true);
		gameObject.SetActive (false);
	}

	public void ResetWindow (bool state) {
		Window.SetActive (state);
	}

	public void GoToCredits () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(MenuState.CREDITS));
		Credits.SetActive (true);
		gameObject.SetActive (false);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Window.activeSelf) {
				Window.SetActive (false);
			}
			else {
				Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
				MainMenu.SetActive (true);
				gameObject.SetActive (false);
			}
		}
	}

	public void BackToMainScreen () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
		MainMenu.SetActive (true);
		gameObject.SetActive (false);
	}
}
