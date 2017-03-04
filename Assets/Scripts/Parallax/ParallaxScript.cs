using UnityEngine;
using System.Collections;

public class ParallaxScript : MonoBehaviour {

	public float speed = 1;
	public float start = 0;
	public float end = 0;

	void Update () {

		transform.localPosition += new Vector3 (0, speed / 1000, 0);
		if (transform.localPosition.y >= end) {
			transform.localPosition = new Vector3 (transform.localPosition.x, start, transform.localPosition.x);
		}
	}
}
