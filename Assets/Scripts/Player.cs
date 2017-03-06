using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class Player : MonoBehaviour
{
	[SoundGroupAttribute] public string CoinSound;
	[SoundGroupAttribute] public string JumpSound;
	[SoundGroupAttribute] public string DeathSound;
	[SoundGroupAttribute] public string TrapSound;

	public float Acceleration = 0;
    public float MaxSpeed = 100;
    public float RotationSpeed = 50;
    public float Jump = 20;
    public LayerMask Ground;
	public Animator _animator;
    public float JumpBuffer = 6;
    public float TrapKillVelocity = 5;
    public float TrapWaitBeforeAnimation = 0.5f;
    public float Radius = 0.5f;
    public float Gravity = 1;
    public float VerticalSpeedOnGround = 2;

    int _direction = 1;
    [HideInInspector]
    public int Direction
    {
        get { return _direction; }
        set
        {
            _direction = value;
			_animator.SetFloat ("Direction", value);
        }
    }

    float _currentJumpBuffer = 0;
    float _distance;
    public bool _isGrounded = false;
    Vector3 oldVelocity;
	SubscriberList _subscriberList = new SubscriberList();
    float _currentSpeed;
	bool _pause = false;
    float _verticalSpeed = 0;
    float _gravityMultiplier = 1;

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
        _distance = new Vector3(transform.position.x, 0, transform.position.z).magnitude;

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
        if (_pause)
            return;
        _currentSpeed += Acceleration * Time.deltaTime;
        if (_currentSpeed > MaxSpeed)
            _currentSpeed = MaxSpeed;

        Event<PlayerMovedEvent>.Broadcast(new PlayerMovedEvent(transform.position, _direction));

        _currentJumpBuffer--;
        if(_isGrounded && _currentJumpBuffer > 0)
        {
            _verticalSpeed = Jump;
            Event<PlayerHaveJumped>.Broadcast(new PlayerHaveJumped());
        }
    }

	void JumpMe (PlayerJumpEvent e)
    {
        if (_isGrounded)
        {
            _cubeFactor = -(Jump / 10);
            Event<PlayerHaveJumped>.Broadcast(new PlayerHaveJumped());
        }
        else _currentJumpBuffer = JumpBuffer;
	}

    void FixedUpdate()
    {
        if (_pause)
            return;

        _gravityMultiplier = _currentSpeed / RotationSpeed;
        _verticalSpeed -= Gravity * Time.deltaTime * _gravityMultiplier;
        
        transform.position = Quaternion.Euler(0, _currentSpeed * Time.deltaTime * _direction, 0) * transform.position;
        Vector3 newPos = transform.position + new Vector3(0, _verticalSpeed * Time.deltaTime, 0);
        
        var hits = Physics.SphereCastAll(new Ray(transform.position, newPos - transform.position), Radius, (newPos - transform.position).magnitude, Ground);
        var hit = new RaycastHit();
        float distance = 10000000.0f;
        foreach(var h in hits)
        {
            hit = h;
            distance = h.distance;
        }
        if (hits.Length == 0)
        {
            transform.position = newPos;
            _isGrounded = CheckGrounded();
            if(_isGrounded)
                _verticalSpeed = -VerticalSpeedOnGround * _gravityMultiplier;
        }
        else
        {
            float d = hit.distance > 0.01f ? hit.distance - 0.01f : 0;
            transform.position = transform.position + (newPos - transform.position).normalized * d;
            _verticalSpeed = -VerticalSpeedOnGround * _gravityMultiplier;
            _isGrounded = true;
        }

        transform.LookAt(new Vector3(0, transform.position.y, 0), Vector3.up);
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
			Event<PlaySoundEvent>.Broadcast(new PlaySoundEvent(TrapSound));
			Event<PlayerMovedEvent>.Broadcast(new PlayerMovedEvent(transform.position, 0));
            enabled = false;
            StartCoroutine(TrapKillCoroutine());
        }

        if (collider.gameObject.tag == "Collectable")
        {
			Event<PlaySoundEvent>.Broadcast(new PlaySoundEvent(CoinSound));
			var collectable = collider.GetComponent<Collectable>();
            Event<CoinCollectedEvent>.Broadcast(new CoinCollectedEvent(collectable != null ? collectable.value : 1));
            collider.gameObject.tag = "Untagged";
            collider.gameObject.GetComponentInChildren<Animator>().SetTrigger("Collect");
            Destroy(collider.gameObject, 0.5f);
        }

        if(collider.gameObject.tag == "TextTrigger")
        {
            var text = collider.GetComponent<TextTrigger>();
            if (text != null)
                Event<TextTriggerEvent>.Broadcast(new TextTriggerEvent(text.text, text.fadeOutTime));
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

    bool CheckGrounded()
    {
        if (_verticalSpeed > 0)
            return false;
        var hits = Physics.SphereCastAll(new Ray(transform.position, new Vector3(0, -1, 0)), Radius, 0.2f, Ground);
        return hits.Length > 0;
    }
}

            _verticalSpeed = Jump;
			Event<PlaySoundEvent>.Broadcast(new PlaySoundEvent(JumpSound));