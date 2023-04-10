using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private Transform SpawnLocation;
    [SerializeField] private GameObject PowerUp;
    private bool PlayerTookPowerUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerTookPowerUp = true; 
        }
    }

    private void Start()
    {
        Instantiate(PowerUp, SpawnLocation.position, SpawnLocation.rotation); 
    }

    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(5);
        Instantiate(PowerUp, SpawnLocation.position, SpawnLocation.rotation); 
    }

    private void Update()
    {
        if(PlayerTookPowerUp)
        {
            StartCoroutine(SpawnItem());
            PlayerTookPowerUp = false; 
        }
    }
}
