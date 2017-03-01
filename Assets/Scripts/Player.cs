using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float RotationSpeed = 50;
	public float GroundGravity = 7;
	public float AirGravityForce = 7;
	public float GravitySpeed = 1;
	public float Jump = 20;
	public LayerMask Ground;
	public LayerMask Ring;
	public Transform GroundCheck;
	[Tooltip("Radius de la sphère qui check si le Player est au sol.")]
	public float GroundCheckRadius = 0.1f;
	public float JumpBuffer = 6;

	int _direction = 1;
	[HideInInspector]
	public int Direction {
		get {
			return _direction;
		}
		set {
			_direction = value;
			GroundCheck.localPosition = new Vector3 (GroundCheck.localPosition.x * value, GroundCheck.localPosition.y, GroundCheck.localPosition.z);
		}
	}
		
	float _gravityBuffer = 0;
	float _cubeFactor;
	float _currentJumpBuffer = 0;
	float _distance;
	Rigidbody _body;
	float _actualGravity = 0;
	public bool _isGrounded = false;
	bool _jumping = false;

	// Use this for initialization
	void Start () {
		_body = GetComponent <Rigidbody> ();
		_distance = new Vector3 (transform.position.x, 0, transform.position.z).magnitude;
	}

	void Update () {
		if (!_isGrounded) {
			_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, Ground);

			if (_isGrounded) {
				_actualGravity = -GroundGravity;
			}
		}
		else {
			_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, Ground);
			if (!_isGrounded) {
				if (!_jumping) {
					_cubeFactor = 0;
					_gravityBuffer = GroundGravity;
				}
				else {
					_jumping = false;
				}
			}
		}

		var touches = Input.touches;

		if (touches.Length > 0) {
			if (touches [0].phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay (touches [0].position);
				RaycastHit hit;
				if (!Physics.Raycast (ray, out hit, 1000, Ring)) {
					if (_isGrounded) {
						_cubeFactor = -(Jump / 10);
						_gravityBuffer = 0;
						_jumping = true;
					}
					else {
						_currentJumpBuffer = JumpBuffer;
					}
				}
			}
		}
		else if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (!Physics.Raycast (ray, out hit, 1000, Ring)) {
				if (_isGrounded) {
					_cubeFactor = -(Jump / 10);
					_gravityBuffer = 0;
					_jumping = true;
				}
				else {
					_currentJumpBuffer = JumpBuffer;
				}
			}
		}
		else if ((_currentJumpBuffer >= 0) && _isGrounded) {
			_cubeFactor = -(Jump / 10);
			_gravityBuffer = 0;
			_jumping = true;
		}



		//DEBUG----------------------------------------------------------------
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene (0, LoadSceneMode.Single);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		_currentJumpBuffer--;

		if (!_isGrounded || _jumping) {
			_cubeFactor += Time.deltaTime;
			_actualGravity = -Mathf.Abs (_cubeFactor) * _cubeFactor * AirGravityForce - _gravityBuffer;
		}

		Vector3 newPosition = Quaternion.Euler (0, RotationSpeed * Time.deltaTime * _direction, 0) * transform.position;
		newPosition = new Vector3 (newPosition.x, 0, newPosition.z);
		newPosition = newPosition.normalized * _distance;
		newPosition = new Vector3 (newPosition.x, transform.position.y + _actualGravity * Time.deltaTime, newPosition.z);
		_body.MovePosition (newPosition);

		transform.LookAt (new Vector3 (0, transform.position.y, 0), Vector3.up);
	}
}
