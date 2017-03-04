using UnityEngine;
using System.Collections;

public class OptionsFunctionnality : MonoBehaviour {

	public GameObject Credits;

	public void ResetData () {
		G.Sys.dataMaster._save = new Save ();
		G.Sys.dataMaster.SetEverything ();
	}

	public void GoToCredits () {
		Credits.SetActive (true);
		gameObject.SetActive (false);
	}
}
