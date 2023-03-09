using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemySlime
{
    [SerializeField] bool isLightOn;
    [SerializeField]AudioSource enemySoundPlayer;
    [SerializeField]AudioClip enemySound; 
    [SerializeField] Animator anime;
    [SerializeField] bool audioStop;
    Rigidbody2D Rigid;
    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        speed = 1;
        chaseSpeed = 1.1f;
        chaseTimerMax = 4.0f;
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.gameObject.CompareTag("flashlight"))
        {
            enemySoundPlayer.clip = enemySound;
            enemySoundPlayer.PlayOneShot(enemySound);
            isLightOn = true;
            anime.SetBool("LightUp", true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        audioStop = false;
        enemySoundPlayer.Stop(); 
        anime.SetBool("lightUp", false);
    }
    void Update()
    {
        Rigid.velocity = new Vector2(0, 0);
        if (isLightOn == true)
        {
            chaseTimer += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, PlayerTarget.position, speed * Time.deltaTime);
            if (chaseTimer >= chaseTimerMax)
            {
                isLightOn = false;
                chaseTimer = 0.0f;
            }
        }

    }


}
