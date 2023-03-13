using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject Fire;

    private void Start()
    {
        Fire.SetActive(false); 
    }
}
