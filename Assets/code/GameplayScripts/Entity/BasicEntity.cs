using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is Basic Entity class, that can be used Basic Enemy, Basic Player, Basic Neautrals
/// </summary>
public class BasicEntity : MonoBehaviour
{
    protected int Health = 10; // Health
    protected int Health_Max = 10; // Health Max
    protected int InvincibilityFrame = 0;
    protected bool Invincible = false;

    protected virtual void Update()
    {
        Update_InvincibilityFrame();
    }

    protected virtual void Update_InvincibilityFrame()
    {
        bool IsInvicible = false;
        if (InvincibilityFrame > 0)
        {
            InvincibilityFrame--;
            IsInvicible = true;
        }
        Invincible = IsInvicible;
    }

    protected virtual void Take_Damage(int Damage_)
    {
        Health -= Damage_;
        if (Health < 0) OnDeath();
    }

    protected virtual void Take_Heal(int Heal_)
    {
        Health += Heal_;
        if (Health> Health_Max) Health = Health_Max;
    }

    protected virtual void OnDeath()
    {
        Destroy(this);
    }
}
