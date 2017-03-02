using UnityEngine;
using System.Collections;

public class ClassicUIActions : MonoBehaviour {

	public GameObject MainMenu;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			MainMenu.SetActive (true);
			gameObject.SetActive (false);
		}
	}

	public void BackToMainScreen () {
		MainMenu.SetActive (true);
		gameObject.SetActive (false);
	}
}
