using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is Basic Entity class, that can be used Basic Enemy, Basic Player, Basic Neautrals
/// </summary>
public class BasicEntity : MonoBehaviour
{
    public float Health = 100; // Health
    public float Health_Max = 100; // Health Max
    public float Invincible_Time = 0;
    protected bool Invincible = false;

    protected virtual void Update()
    {
        Update_InvincibilityFrame();
    }

    protected virtual void Update_InvincibilityFrame()
    {
        bool IsInvicible = false;
        if (Invincible_Time > 0)
        {
            Invincible_Time--;
            IsInvicible = true;
        }
        Invincible = IsInvicible;
    }

    public virtual void Take_Damage(float Damage_)
    {
        Health -= Damage_;
        if (Health < 0) OnDeath();
    }

    public virtual void Take_Heal(float Heal_)
    {
        Health += Heal_;
        if (Health> Health_Max) Health = Health_Max;
    }

    public virtual void OnDeath()
    {
        Object.Destroy(this.gameObject);
    }
}
