using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source:
//https://www.youtube.com/watch?v=Pa0VFJGxE-0

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
