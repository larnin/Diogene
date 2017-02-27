using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMovement : MonoBehaviour {

	public float RingTurnSpeed = 1f;

	float storedPosition;

	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			storedPosition = Input.mousePosition.x;
		}
		else if (Input.GetMouseButton (0)) {
			transform.rotation *= Quaternion.Euler (0, -RingTurnSpeed * (Input.mousePosition.x - storedPosition) * Time.deltaTime, 0);
			storedPosition = Input.mousePosition.x;
		}
	}
}
