using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public LayerMask layerMask;
	public float RoundVelocity = 1f;
	public float DownVelocity = 1f;
	public float RingTurnSpeed = 1f;

	Transform currentRing;
	float storedPosition;

	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3(0, -DownVelocity * Time.deltaTime, 0));
		transform.parent.transform.rotation *= Quaternion.Euler (0, RoundVelocity * Time.deltaTime, 0);

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			Debug.DrawRay (ray.origin, ray.direction, Color.blue);
			if (Physics.Raycast (ray, out hit, 1000, layerMask)) {
				if (hit.collider.tag == "MasterRing") {
					currentRing = hit.collider.transform;
				}
				else {
					currentRing = hit.collider.transform.parent.transform;
				}

				storedPosition = Input.mousePosition.x;
			}
		}
		else if (Input.GetMouseButtonUp (0)) {
			currentRing = null;
		}
		else if (Input.GetMouseButton (0) && (currentRing != null)) {
			currentRing.rotation *= Quaternion.Euler (0, -RingTurnSpeed * (Input.mousePosition.x - storedPosition) * Time.deltaTime, 0);
			storedPosition = Input.mousePosition.x;
		}
	}
}
