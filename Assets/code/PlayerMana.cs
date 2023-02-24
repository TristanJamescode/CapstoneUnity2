using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public GameObject ManaBar;

    private float mana;

    public float maxMana = 100;

    private void Start()
    {
        mana = maxMana;
    }

    public void AddMana(float amount)
    {
        mana += amount;
        Debug.Log("Mana Decreased, Current Mana: " + mana);
        UpdateManaBar();
    }

    public void DecreasedMana(float amount)
    {
        mana -= amount;
        Debug.Log("Mana Decreased, Current Mana: " + mana);
        if (mana <= 0)
        {
            mana = 0;
            Debug.Log("Mana is gone");
        }
        UpdateManaBar();
    }

    public void UpdateManaBar()
    {
        float fillAmount = mana / maxMana;
        if (fillAmount > 1)
        {
            fillAmount = 1;
        }
        ManaBar.GetComponent<Image>().fillAmount = fillAmount;
    }
}
