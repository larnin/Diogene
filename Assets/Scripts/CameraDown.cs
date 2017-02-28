using UnityEngine;
using System.Collections;

public class CameraDown : MonoBehaviour
{
    public float speed = 1;

	void Start ()
    {
        Event<InitializeEvent>.Broadcast(new InitializeEvent(transform.position));
	}
	
	void Update ()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        Event<CameraMovedEvent>.Broadcast(new CameraMovedEvent(transform.position));
	}
}
