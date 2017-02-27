using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		var touches = Input.touches;

		if (touches.Length > 0) {
			if (touches[0].phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay (touches[0].position);
				RaycastHit hit;
				Debug.DrawRay (ray.origin, ray.direction, Color.blue);
				if (Physics.Raycast (ray, out hit, 1000, layerMask)) {
					if (hit.collider.tag == "MasterRing") {
						currentRing = hit.collider.transform;
					}
					else {
						currentRing = hit.collider.transform.parent.transform;
					}

					storedPosition = touches[0].position.x;
				}
			}
			else if (touches[0].phase == TouchPhase.Ended) {
				currentRing = null;
			}
			else if (currentRing != null) {
				currentRing.rotation *= Quaternion.Euler (0, -RingTurnSpeed * (touches[0].position.x - storedPosition) * Time.deltaTime, 0);
				storedPosition = touches[0].position.x;
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene (0, LoadSceneMode.Single);
		}
	}
}
