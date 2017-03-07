using UnityEngine;
using System.Collections;

public enum DirectionToGo { GoLeft = 1, GoRight = -1 };

public class ChangeDirectionWall : MonoBehaviour
{
    public DirectionToGo direction;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<Player>().Direction = (int)direction;
        }
    }
}