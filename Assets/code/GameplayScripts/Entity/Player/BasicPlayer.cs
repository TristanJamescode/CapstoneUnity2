using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasicPlayer : BasicEntity
{
    public GameObject HealthBar;
    protected enum ANIMSTATE
    {
        IDLE,
        WALK,
        JUMP,
        ATTACK,
    }
    public override void OnDeath()
    {
        
    }
    public override void Take_Heal(float Heal_)
    {
        base.Take_Heal(Heal_);
        UpdateHealthBar();
    }
    public override void Take_Damage(float Damage_)
    {
        base.Take_Damage(Damage_);
        UpdateHealthBar();
    }
    public virtual void UpdateHealthBar()
    {
        float fillAmount = Health / Health_Max;
        if (fillAmount > 1)
        {
            fillAmount = 1;
        }
        HealthBar.GetComponent<Image>().fillAmount = fillAmount;
    }
}
