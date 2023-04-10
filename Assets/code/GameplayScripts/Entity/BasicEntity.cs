using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is Basic Entity class, that can be used Basic Enemy, Basic Player, Basic Neautrals
/// </summary>
public class BasicEntity : MonoBehaviour
{
    [Header("Health")]
    public float Health = 100; // Health
    public float Health_Max = 100; // Health Max
    protected float Invincible_Time = 0;
    protected bool Invincible = false;
    //knockback value
    protected Vector3 Knockback_Velocity;
    protected float Knockback_Counter = 0;
    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
        Update_InvincibilityFrame();
        Update_KnockbackRelated();
    }
    protected virtual void Update_InvincibilityFrame()
    {
        bool IsInvicible = false;
        if (Invincible_Time > 0)
        {
            Invincible_Time-=Time.deltaTime;
            IsInvicible = true;
        }
        Invincible = IsInvicible;
    }

    public virtual IEnumerator Burn()
    {
        yield return new WaitForSeconds(3);
        Take_Damage(40.0f);
        yield return new WaitForSeconds(3);
        Take_Damage(40.0f);
        yield return new WaitForSeconds(3);
        Take_Damage(40.0f); 
    }
    public virtual void Update_KnockbackRelated(){
        if (Knockback_Counter > 0) Knockback_Counter -= Time.deltaTime;
    }
    public virtual void Take_Damage(float Damage_)
    {
        if (Invincible) return;
        Health -= Damage_;
        if (Health <= 0) OnDeath();
    }
    public virtual void Take_Heal(float Heal_)
    {
        Health += Heal_;
        if (Health> Health_Max) Health = Health_Max;
    }
    public virtual void Take_Knockback(float Amount, Vector3 Direction) 
    {
    }
    public virtual void OnDeath()
    {
        Object.Destroy(this.gameObject);
    }
}
