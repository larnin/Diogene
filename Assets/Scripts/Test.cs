using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    private SubscriberList _subscriberList = new SubscriberList();

    void Awake()
    {
        _subscriberList.Add(new Event<TestEvent>.Subscriber(OnTestEvent));
        _subscriberList.Subscribe();
    }

	void Start ()
    {
        Event<TestEvent>.Broadcast(new TestEvent("It work !"));
	}
	
	void Update ()
    {
	
	}

    void OnTestEvent(TestEvent e)
    {
        Debug.Log(e.text);
    }
}
