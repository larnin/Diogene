using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float Acceleration = 0;
    public float MaxSpeed = 100;
    public float RotationSpeed = 50;
    public float GroundGravity = 7;
    public float AirGravityForce = 7;
    public float GravitySpeed = 1;
    public float Jump = 20;
    public LayerMask Ground;
    public LayerMask Ring;
    public Transform GroundCheck;
	public Animator _animator;
    [Tooltip("Radius de la sphère qui check si le Player est au sol.")]
    public float GroundCheckRadius = 0.1f;
    public float JumpBuffer = 6;
    public float TimeBeforeDestroy = 2;
    public float TrapKillVelocity = 5;
    public float TrapWaitBeforeAnimation = 0.5f;

    int _direction = 1;
    [HideInInspector]
    public int Direction
    {
        get { return _direction; }
        set
        {
            _direction = value;
            GroundCheck.localPosition = new Vector3(_groundCheckX * value, GroundCheck.localPosition.y, GroundCheck.localPosition.z);
			_animator.SetFloat ("Direction", value);
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
    float _groundCheckX;
    Vector3 oldVelocity;
	SubscriberList _subscriberList = new SubscriberList();
    float _currentSpeed;
	bool _pause = false;

	void Awake()
	{
		_subscriberList.Add (new Event<ResetEvent>.Subscriber (OnReset));
		_subscriberList.Add (new Event<PausePlayerEvent>.Subscriber (Pause));
		_subscriberList.Add (new Event<PlayerJumpEvent>.Subscriber (JumpMe));
        _currentSpeed = RotationSpeed;
    }

	void OnEnable()
	{
		_subscriberList.Subscribe ();
	}

	void OnDisable()
	{
		_subscriberList.Unsubscribe();
	}

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _distance = new Vector3(transform.position.x, 0, transform.position.z).magnitude;
        _groundCheckX = GroundCheck.localPosition.x;

        Event<PlayerMovedEvent>.Broadcast(new PlayerMovedEvent(transform.position, 0));
        Event<InstantMoveCameraEvent>.Broadcast(new InstantMoveCameraEvent());
    }

	void Pause (PausePlayerEvent e) {
		_pause = e.State;
		if (_pause) {
			_animator.SetFloat ("Direction", 0);
		}
		else {
			_animator.SetFloat ("Direction", Direction);
		}
	}

    void Update()
    {
		if (!_pause) {
			
			_currentSpeed += Acceleration * Time.deltaTime;
			if (_currentSpeed > MaxSpeed)
				_currentSpeed = MaxSpeed;
			
			if (!_isGrounded)
			{
				_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, Ground);
				
				if (_isGrounded)
				{
					_actualGravity = -GroundGravity;
				}
			}
			else
			{
				_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, Ground);
				if (!_isGrounded)
				{
					if (!_jumping)
					{
						_cubeFactor = 0;
						_gravityBuffer = GroundGravity;
					}
					else
					{
						_jumping = false;
					}
				}
			}

			if ((_currentJumpBuffer >= 0) && _isGrounded)
			{
				_cubeFactor = -(Jump / 10);
				_gravityBuffer = 0;
				_jumping = true;
			}
			
			Event<PlayerMovedEvent>.Broadcast(new PlayerMovedEvent(transform.position, _direction));
		}
    }

	void JumpMe (PlayerJumpEvent e) {
		if (_isGrounded)
		{
			_cubeFactor = -(Jump / 10);
			_gravityBuffer = 0;
			_jumping = true;
		}
		else
		{
			_currentJumpBuffer = JumpBuffer;
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (!_pause) {

			_currentJumpBuffer--;
			
			if (!_isGrounded || _jumping)
			{
				_cubeFactor += Time.deltaTime;
				_actualGravity = -Mathf.Abs(_cubeFactor) * _cubeFactor * AirGravityForce - _gravityBuffer;
			}
			
			Vector3 newPosition = Quaternion.Euler(0, _currentSpeed * Time.deltaTime * _direction, 0) * transform.position;
			newPosition = new Vector3(newPosition.x, 0, newPosition.z);
			newPosition = newPosition.normalized * _distance;
			newPosition = new Vector3(newPosition.x, transform.position.y + _actualGravity * Time.deltaTime, newPosition.z);
			oldVelocity = (newPosition - transform.position) / Time.deltaTime;
			_body.MovePosition(newPosition);
			
			transform.LookAt(new Vector3(0, transform.position.y, 0), Vector3.up);
		}

    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "KillGrid")
        {
            Event<PlayerKillEvent>.Broadcast(new PlayerKillEvent(_currentSpeed));
            Event<PlayerMovedEvent>.Broadcast(new PlayerMovedEvent(transform.position, 0));
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.velocity = oldVelocity;
            enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
            Destroy(gameObject, 2);
        }

        if(collider.gameObject.tag == "Trap")
        {
            Event<PlayerMovedEvent>.Broadcast(new PlayerMovedEvent(transform.position, 0));
            enabled = false;
            StartCoroutine(TrapKillCoroutine());
        }

        if (collider.gameObject.tag == "Collectable")
        {
            var collectable = collider.GetComponent<Collectable>();
            Event<CoinCollectedEvent>.Broadcast(new CoinCollectedEvent(collectable != null ? collectable.value : 1));
            collider.gameObject.tag = "Untagged";
            collider.gameObject.GetComponentInChildren<Animator>().SetTrigger("Collect");
            Destroy(collider.gameObject, 0.5f);
        }
    }

    IEnumerator TrapKillCoroutine()
    {
        yield return new WaitForSeconds(TrapWaitBeforeAnimation);
        Event<PlayerKillEvent>.Broadcast(new PlayerKillEvent(_currentSpeed));
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        var dir = (Vector3.Cross(new Vector3(transform.position.x, 0, transform.position.z), Vector3.up).normalized + Vector3.up.normalized).normalized;
        if (_direction < 0)
        {
            dir.x *= -1;
            dir.z *= -1;
        }
        rigidBody.velocity = dir * TrapKillVelocity;
        enabled = false;
        GetComponentInChildren<Collider>().enabled = false;
        Destroy(gameObject, 3);
    }

	void OnReset(ResetEvent e)
	{
		Destroy (gameObject);
	}
}
