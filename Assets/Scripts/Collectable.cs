using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Collectable : MonoBehaviour
{
    public int value;

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
