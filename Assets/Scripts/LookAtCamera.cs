using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
