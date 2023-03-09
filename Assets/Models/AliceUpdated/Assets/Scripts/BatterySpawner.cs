using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [SerializeField] bool timerStart;
    [SerializeField] float spawnTimer, spawnTimerMax;
    [SerializeField] GameObject [] itemArray;
    // Start is called before the first frame update
    void Start()
    {
        timerStart = false;
        spawnTimer = 0.0f;
        spawnTimerMax = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnTimerMax)
            {
                int randomItem = Random.Range(0, itemArray.Length - 1);
                Instantiate(itemArray[randomItem], transform.position,Quaternion.identity);
                timerStart = false;
                spawnTimer = 0.0f;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.tag);
        if(col.tag == "Player")
        {
            timerStart = true;
        }
    }
}
