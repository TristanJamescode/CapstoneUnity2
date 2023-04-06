using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//link to original code: https://www.youtube.com/watch?v=I7M8T3qU-_E
public class Portal : MonoBehaviour
{
    [SerializeField] Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<BasicPlayer>(out var player)){ 
        player.Teleport(destination.position, destination.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(destination.position, 0.4f);
        var direction = destination.TransformDirection(Vector3.forward);    
        Gizmos.DrawRay(direction, destination.position);
    }
}
