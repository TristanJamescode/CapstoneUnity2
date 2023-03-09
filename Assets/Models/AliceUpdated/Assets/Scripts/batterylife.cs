using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class batterylife : MonoBehaviour {

    Image lifeofbattery;
    public float maxbatterypower = 100f;
    public float minbatterypower;

    //accessing light;
    [SerializeField]
    GameObject Lightcone;

    // Use this for initialization
    void Start () {

        lifeofbattery = GetComponent<Image>();
        minbatterypower = maxbatterypower;
        Lightcone = GameObject.FindGameObjectWithTag("flashlight");
    }
	
	// Update is called once per frame
	void Update () {
        if (minbatterypower > 0)
        {
            minbatterypower -= Time.deltaTime * 2;
            lifeofbattery.fillAmount = minbatterypower / maxbatterypower;

        }
        else {

            //Time.timeScale = 0;
            Batterydrain();
        }
    }


    public void Batterydrain()
    {
        if (minbatterypower <= 0)
        {
            minbatterypower = 0;
            Lightcone.SetActive(false);
        }
        else
        {
            Lightcone.SetActive(true);
        }
    }


  
}
