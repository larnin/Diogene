using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {

	[HideInInspector]
	public Vector3 AimedRotation;

	void Start () {
		AimedRotation = transform.eulerAngles;
	}
}
