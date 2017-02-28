using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

	public GameObject PlayerPhysic;
	public float RotationSpeed = 50;

	float distance;

	// Use this for initialization
	void Start () {
		distance = new Vector3 (PlayerPhysic.transform.position.x, 0, PlayerPhysic.transform.position.z).magnitude;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 newPosition = Quaternion.Euler (0, RotationSpeed * Time.deltaTime, 0) * PlayerPhysic.transform.position;
		newPosition = new Vector3 (newPosition.x, 0, newPosition.z);
		newPosition = newPosition.normalized * distance;
		newPosition = new Vector3 (newPosition.x, PlayerPhysic.transform.position.y, newPosition.z);
		PlayerPhysic.GetComponent <Rigidbody>().MovePosition (newPosition);

		transform.LookAt (new Vector3 (0, PlayerPhysic.transform.position.y, 0), transform.forward + transform.right);
	}
}
