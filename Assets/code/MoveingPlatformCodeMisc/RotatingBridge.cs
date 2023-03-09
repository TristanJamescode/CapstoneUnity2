using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBridge : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0.0f, 1.0f, 0.0f, Space.Self); 
    }
}
