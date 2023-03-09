using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundSpin : MonoBehaviour
{
    [SerializeField] float angle;
    // Start is called before the first frame update
    void Start()
    {
        if(angle == 0)
        {
            angle = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(angle * new Vector3(0, 0, 1) * Time.deltaTime);
    }
}
