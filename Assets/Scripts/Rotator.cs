using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;
	SubscriberList _subscriberList = new SubscriberList();
	bool _pause = false;

	// Use this for initialization
	void Awake () {
		_subscriberList.Add (new Event<PauseRingEvent>.Subscriber (Pause));
		_subscriberList.Subscribe ();
	}

	void Pause (PauseRingEvent e) {
		_pause = e.State;
	}

    void Update()
    {
		if (!_pause) {
			transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
		}
    }
	
}
