using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RingInput : MonoBehaviour {

	public float RingTurnForce = 10;
	public float RingRotationSpeed = 0.25f;
	public int NbCran = 16;
	public LayerMask RingLayer;

	float _totalRotation;
	float storedPosition;
	Transform currentRing;
	Ring currentScript;
	float _inputBuffer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var touches = Input.touches;

		if (touches.Length > 0) {
			if (touches[0].phase == TouchPhase.Began) {
				_inputBuffer = 0;
				Ray ray = Camera.main.ScreenPointToRay (touches[0].position);
				RaycastHit hit;
				Debug.DrawRay (ray.origin, ray.direction, Color.blue);
				if (Physics.Raycast (ray, out hit, 1000, RingLayer)) {
					currentRing = hit.collider.transform;
					currentScript = currentRing.GetComponent <Ring> ();

					storedPosition = touches[0].position.x;
				}
			}
			else if (touches[0].phase == TouchPhase.Ended) {
				currentRing = null;
				currentScript = null;
			}
			else if (currentRing != null) {
				_inputBuffer += -RingTurnForce * (touches[0].position.x - storedPosition) * Time.deltaTime;
				if (_inputBuffer >= (360 / NbCran)) {
					_inputBuffer -= 360 / NbCran;
					currentRing.DOKill ();
					currentScript.AimedRotation.y += 360 / NbCran;
					currentRing.DORotate (currentScript.AimedRotation, RingRotationSpeed);
				}
				else if (_inputBuffer <= -(360 / NbCran)) {
					_inputBuffer += 360 / NbCran;
					currentRing.DOKill ();
					currentScript.AimedRotation.y -= 360 / NbCran;
					currentRing.DORotate (currentScript.AimedRotation, RingRotationSpeed);
				}
				storedPosition = touches[0].position.x;
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			_inputBuffer = 0;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			Debug.DrawRay (ray.origin, ray.direction, Color.blue);
			if (Physics.Raycast (ray, out hit, 1000, RingLayer)) {
				currentRing = hit.collider.transform;
				currentScript = currentRing.GetComponent <Ring> ();

				storedPosition = Input.mousePosition.x;
			}
		}
		else if (Input.GetMouseButtonDown (0)) {
			currentRing = null;
			currentScript = null;
		}
		else if (Input.GetMouseButton(0) && currentRing != null) {
			_inputBuffer += -RingTurnForce * 2 * (Input.mousePosition.x - storedPosition) * Time.deltaTime;
			if (_inputBuffer >= (360 / NbCran)) {
				_inputBuffer -= 360 / NbCran;
				currentRing.DOKill ();
				currentScript.AimedRotation.y += 360 / NbCran;
				currentRing.DORotate (currentScript.AimedRotation, RingRotationSpeed);
			}
			else if (_inputBuffer <= -(360 / NbCran)) {
				_inputBuffer += 360 / NbCran;
				currentRing.DOKill ();
				currentScript.AimedRotation.y -= 360 / NbCran;
				currentRing.DORotate (currentScript.AimedRotation, RingRotationSpeed);
			}
			storedPosition = Input.mousePosition.x;
		} 
	}
}
