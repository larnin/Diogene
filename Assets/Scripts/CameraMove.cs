using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public float RoundVelocity = 1f;
	public float DownVelocity = 1f;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3(0, -DownVelocity * Time.deltaTime, 0));
		transform.parent.transform.rotation *= Quaternion.Euler (0, RoundVelocity * Time.deltaTime, 0);
	}
}
