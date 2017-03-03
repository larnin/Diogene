using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContinueFonctionnality : MonoBehaviour {

	public GameObject GameOverScreen;
	public Text Chrono;
	public float Timer = 5;

	float timer;

	void Start () {
		timer = Timer;
	}

	void OnEnable () {
		timer = Timer;
	}

	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		Chrono.text = Mathf.RoundToInt (timer).ToString ();

		if (timer <= -0.5f) {
			GameOverScreen.SetActive (true);
			gameObject.SetActive (false);
		}
	}
}
