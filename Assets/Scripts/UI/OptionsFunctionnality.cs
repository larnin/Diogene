using UnityEngine;
using System.Collections;

public class OptionsFunctionnality : MonoBehaviour {

	public void ResetData () {
		G.Sys.dataMaster._save = new Save ();
		G.Sys.dataMaster.SetEverything ();
	}
}
