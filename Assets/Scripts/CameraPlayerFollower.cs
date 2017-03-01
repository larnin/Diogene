using UnityEngine;
using System.Collections;

public class CameraPlayerFollower : MonoBehaviour
{
    //public GameObject player;
    public float angleOffset;
    public float verticalOffset;
    public float distance;
    public float moveSpeed;

    float targetAngle;
    float targetHeight;
    bool targetSet = false;
    //Player playerScript;

    SubscriberList _subscriberList = new SubscriberList();

    void Awake()
    {
        _subscriberList.Add(new Event<PlayerMovedEvent>.Subscriber(OnPlayerMove));
        _subscriberList.Add(new Event<InstantMoveCameraEvent>.Subscriber(OnInstantMove));
        _subscriberList.Subscribe();
    }

	void Start ()
    {
        /*playerScript = player.GetComponent<Player>();
        targetAngle = angleFrom(player.transform.position);
        targetHeight = player.transform.position.y + verticalOffset;
        InstantMove();*/
    }

    void LateUpdate()
    {
        if(targetSet)
            SmoothMove();
    }

    float angleFrom(Vector3 pos)
    {
        return Mathf.Atan2(pos.z, pos.x);
    }

    float clampAngle(float angle)
    {
        while (angle > Mathf.PI)
            angle -= 2 * Mathf.PI;
        while (angle < -Mathf.PI)
            angle += 2 * Mathf.PI;
        return angle;

    }

    void SmoothMove()
    {
        float currentAngle = angleFrom(transform.position);
        float offset = clampAngle(targetAngle - currentAngle);
        currentAngle += (offset) * Time.deltaTime * moveSpeed;
        transform.position = new Vector3(Mathf.Cos(currentAngle) * distance, targetHeight, Mathf.Sin(currentAngle) * distance);
        transform.LookAt(new Vector3(0, targetHeight, 0));

    }

    void InstantMove()
    {
        transform.position = new Vector3(Mathf.Cos(targetAngle) * distance, targetHeight, Mathf.Sin(targetAngle) * distance);
        transform.LookAt(new Vector3(0, targetHeight, 0));
    }

    void OnPlayerMove(PlayerMovedEvent e)
    {
        targetSet = true;
        targetAngle = angleFrom(e.pos) + (e.direction > 0 ? angleOffset : -angleOffset);
        targetHeight = e.pos.y;
    }

    void OnInstantMove(InstantMoveCameraEvent e)
    {
        InstantMove();
    }
}
