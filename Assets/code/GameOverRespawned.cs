using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRespawned : MonoBehaviour
{

    [SerializeField] GameObject GameOverScreen;
    public float threshold;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            GameOverScreen.SetActive(true);
            Destroy(gameObject);
        }
    }
}
