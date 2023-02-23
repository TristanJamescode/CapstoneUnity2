using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Sources:
//https://www.youtube.com/watch?v=Pt-L8cBO4GQ&t=903s
//https://www.youtube.com/watch?v=Ca4J-_zIBuE

public class PlayerHealth : MonoBehaviour
{

    public GameObject HealthBar;

    private float health;

    public float maxHealth = 100;

    private void Start()
    {
        health = maxHealth;        
    }

    public void AddHealth(float amount)
    {
        health+= amount;
        Debug.Log("Health Decreased, Current Health: " + health);
        UpdateHealthBar();
    }

    public void DecreasedHealth(float amount)
    {
        health -= amount;
        Debug.Log("Health Decreased, Current Health: " + health);
        if(health <= 0) { 
        health = 0;
            Debug.Log("Player is dead");
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float fillAmount = health/maxHealth;    
        if(fillAmount > 1)  {
            fillAmount = 1;
             }
        HealthBar.GetComponent<Image>().fillAmount = fillAmount;     
    }

    //public void AddHealthBar(float amount)
    //{
    //    float fillAmount = health / maxHealth;
    //    if (fillAmount > 1)
    //    {
    //        fillAmount = 1;
    //    }
    //    HealthBar.GetComponent<Image>().fillAmount = fillAmount;
    //}

}
