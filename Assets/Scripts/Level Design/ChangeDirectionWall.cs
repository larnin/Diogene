using UnityEngine;
using System.Collections;

public enum DirectionToGo {GoLeft = 1, GoRight = -1};

public class ChangeDirectionWall : MonoBehaviour {

	public DirectionToGo direction;

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Player") {
			collision.collider.GetComponentsInParent<Player> ()[0].Direction = (int)direction;
		}
	}
}
