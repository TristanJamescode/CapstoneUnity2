using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//referance: https://www.youtube.com/watch?v=5p-bNLkQu94

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    private float time = 20;

    private Rigidbody rb;
    private Vector3 currentPosition;
    CharacterController cc; 

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        currentPosition = Vector3.Lerp(start.position, end.position, Mathf.Cos(Time.time / time * Mathf.PI * 2) * -0.5f * 5f);
        rb.MovePosition(currentPosition); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            cc = other.GetComponent<CharacterController>();
            Debug.Log("Got Char Controller: " + cc); 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(cc.Move(rb.velocity * Time.deltaTime)); 
        }
    }
}