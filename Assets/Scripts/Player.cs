using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

	public float RotationSpeed = 50;
	public float Gravity = 9;
	public float GravitySpeed = 1;
	public float Jump = 20;
	public LayerMask Ground;
	public Transform GroundCheck;
	[Tooltip("Radius de la sphère qui check si le Player est au sol.")]
	public float GroundCheckRadius = 0.1f;
	public float JumpBuffer = 6;

	[HideInInspector]
	public int Direction = 1;

	float _currentJumpBuffer = 0;
	float _distance;
	Rigidbody _body;
	float _actualGravity = 0;
	bool _isGrounded = false;
	bool _canJump = false;

	// Use this for initialization
	void Start () {
		_body = GetComponent <Rigidbody> ();
		_distance = new Vector3 (transform.position.x, 0, transform.position.z).magnitude;
	}

	void Update () {
		if (!_isGrounded) {
			_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, Ground);
			_canJump = false;

			if (_isGrounded) {
				_actualGravity = -Gravity;
				_canJump = true;
			}
		}
		else {
			_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, Ground);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (_isGrounded) {
				_canJump = false;
				_actualGravity = Jump;
			}
			else {
				_currentJumpBuffer = JumpBuffer;
			}
		}
		else if ((_currentJumpBuffer >= 0) && _isGrounded) {
			_canJump = false;
			_actualGravity = Jump;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		_currentJumpBuffer--;

		if (!_isGrounded) {
			_actualGravity -= (Gravity / GravitySpeed) * Time.deltaTime;
			if (_actualGravity < -Gravity * 2) {
				_actualGravity = -Gravity * 2;
			}
		}

		Vector3 newPosition = Quaternion.Euler (0, RotationSpeed * Time.deltaTime * Direction, 0) * transform.position;
		newPosition = new Vector3 (newPosition.x, 0, newPosition.z);
		newPosition = newPosition.normalized * _distance;
		newPosition = new Vector3 (newPosition.x, transform.position.y + _actualGravity * Time.deltaTime, newPosition.z);
		_body.MovePosition (newPosition);

		transform.LookAt (new Vector3 (0, transform.position.y, 0), Vector3.up);
	}
}
