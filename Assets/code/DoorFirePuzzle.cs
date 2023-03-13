using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFirePuzzle : MonoBehaviour
{
    [SerializeField] GameObject fireOne;
    [SerializeField] GameObject fireTwo;
    [SerializeField] GameObject portal; 

    private void Update()
    {
        if(fireOne.activeSelf && fireTwo.activeSelf)
        {
            portal.SetActive(true); 
        }
    }
}
