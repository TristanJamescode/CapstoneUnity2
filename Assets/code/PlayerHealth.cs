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

    [SerializeField] AudioSource AudioS; 

    private void Start()
    {
        health = maxHealth;        
    }

    private void Death()
    {
        AudioS.Play();
        Debug.Log("Player Is Dead"); 
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
        if(health <= 0) 
        { 
            health = 0;
            Death(); 
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
