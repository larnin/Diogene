using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

	public float RotationSpeed = 50;
	public float Gravity = 9;
	public float GravitySpeed = 1;
	public float Jump = 20;

	float _distance;
	Rigidbody _body;
	float _actualGravity = 0;

	// Use this for initialization
	void Start () {
		_body = GetComponent <Rigidbody> ();
		_distance = new Vector3 (transform.position.x, 0, transform.position.z).magnitude;
	}

	void Update () {



		if (Input.GetKeyDown (KeyCode.Space)) {
			_actualGravity = Jump;
		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		_actualGravity -= (Gravity / GravitySpeed) * Time.deltaTime;
		if (_actualGravity < -Gravity) {
			_actualGravity = -Gravity;
		}

		Vector3 newPosition = Quaternion.Euler (0, RotationSpeed * Time.deltaTime, 0) * transform.position;
		newPosition = new Vector3 (newPosition.x, 0, newPosition.z);
		newPosition = newPosition.normalized * _distance;
		newPosition = new Vector3 (newPosition.x, transform.position.y + _actualGravity * Time.deltaTime, newPosition.z);
		_body.MovePosition (newPosition);

		transform.LookAt (new Vector3 (0, transform.position.y, 0), Vector3.up);
	}
}
