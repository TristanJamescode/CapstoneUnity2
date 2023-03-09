using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    //sets variables eqaul to game objects attached in unity. I placed different heart images of a heart depleting here. 
    public GameObject pillow;
    public GameObject pillow2;
    public GameObject pillow3;
    //creats player life variable. Is static so I can use this variable in another script.  
    public static int life;

    void Start()
    {
        //sets all parts of health active at the start.  
        life = 5;
        pillow.gameObject.SetActive(true);
        pillow2.gameObject.SetActive(true);
        pillow3.gameObject.SetActive(true);
    }

    void Update()
    {
        if (life > 3)
            life = 3;

        switch (life)
        {
            //these case statements erase a heart image causeing the heart in game to empty every time life goes down one. 
            case 3:
                pillow.gameObject.SetActive(true);
                pillow2.gameObject.SetActive(true);
                pillow3.gameObject.SetActive(true);
                break;
            case 2:
                pillow.gameObject.SetActive(true);
                pillow2.gameObject.SetActive(true);
                pillow3.gameObject.SetActive(false);
                break;
            case 1:
                pillow.gameObject.SetActive(true);
                pillow2.gameObject.SetActive(false);
                pillow3.gameObject.SetActive(false);
                break;
            case 0:
                pillow.gameObject.SetActive(false);
                pillow2.gameObject.SetActive(false);
                pillow3.gameObject.SetActive(false);
                //sets timescale to zero and loads the gameover scene when all heart gameobjects are set inactive. 
                Time.timeScale = 0;
                SceneManager.LoadScene("GameOver");
                break;
        }
    }
}