using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    // dark world
    [SerializeField]
    GameObject darkworld;

    // normal world
    [SerializeField]
    GameObject world;

    [SerializeField]
    GameObject button;

    [SerializeField]
    GameObject pillows;

    [SerializeField]
    GameObject battery;

    // Start is called before the first frame update
    void Awake()
    {
        darkworld = GameObject.FindGameObjectWithTag("darkworld");
        //world = GameObject.FindGameObjectWithTag("world");
    }
   

    // Update is called once per frame
    void Update() {}
  

    public void switchWorlds()
    {
        if (darkworld.activeInHierarchy)
        {
            world.SetActive(true);
            button.SetActive(true);
            darkworld.SetActive(false);
            pillows.SetActive(false);
            battery.SetActive(false);
        }
        else
        {
            world.SetActive(true);
            darkworld.SetActive(false);
        }
    }
    
}
