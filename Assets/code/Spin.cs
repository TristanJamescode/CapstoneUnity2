using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Source:
//https://www.youtube.com/watch?v=K3Ap2_beGnE
public class Spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0.5f, 0f, Space.Self);
    }
}
