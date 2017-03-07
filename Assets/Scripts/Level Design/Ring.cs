using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour
{
    public int NbCran = 8;
    public float RotationTime = 0.5f;

    int current = 0;
    float offset = 0;
    SubscriberList _subscriberList = new SubscriberList();
    Coroutine _coroutine;

    void Awake()
    {
        _subscriberList.Add(new Event<MoveRingEvent>.Subscriber(OnMove));
        _subscriberList.Subscribe();
    }

	void Start ()
    {
        //AimedRotation = transform.eulerAngles;
        offset = transform.eulerAngles.y;
	}

    void OnDisable()
    {
        _subscriberList.Unsubscribe();
    }

    void OnMove(MoveRingEvent e)
    {
        current += e.direction;
        while (current >= NbCran)
            current -= NbCran;
        while (current < 0)
            current += NbCran;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        StartCoroutine(rotateCoroutine(current * 360.0f / NbCran + offset));

        /*target = current * 360.0f / NbCran + offset;

        var rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rot.x, current * 360.0f / NbCran, rot.z);*/
    }

    IEnumerator rotateCoroutine(float target)
    {
        float totalTime = 0;
        float startRot = transform.rotation.eulerAngles.y;
        if (target - startRot > 180)
            target -= 360;
        if (target - startRot < -180)
            target += 360;
        while (totalTime < RotationTime)
        {
            yield return new WaitForFixedUpdate();
            totalTime += Time.deltaTime;
            float current = startRot + (target - startRot) * totalTime / RotationTime;
            var rot = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rot.x, current, rot.z);
        }
        yield return new WaitForFixedUpdate();
        var r = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(r.x, target, r.z);
    }

    [HideInInspector]
    public Vector3 AimedRotation;
}
