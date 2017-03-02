using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsInTheBank : MonoBehaviour {

	Text _myText;

	void Start () {
		_myText = GetComponent<Text> ();
	}

	void Update () {
		_myText.text = G.Sys.dataMaster.CoinsText;
	}
}
