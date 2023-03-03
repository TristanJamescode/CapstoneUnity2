using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasicPlayer : BasicEntity
{
    private CharacterController controller;
    public float Mana = 100; // Mana
    public float Mana_max = 100; // Mana Max
    private bool IsAlive = true; 
    //public float Stamina = 100;
    //public float Stamina_max = 100;
    protected float Mana_Regen_Amount = 1;
    protected float Mana_Regen_Delay = 10;
    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject ManaBar;
    [SerializeField] AudioSource DeathSound;
    [SerializeField] Animator myAnim;
    protected override void Update()
    {
        base.Update();
        //Mana_Regen();
        if (Knockback_Velocity != Vector3.zero)
        {
            Knockback_Velocity *= 0.9f;
            controller = GetComponent<CharacterController>();
            controller.Move(Knockback_Velocity * Time.deltaTime);
            if (Knockback_Velocity.magnitude < 0.5f) Knockback_Velocity = Vector3.zero;
        }
    }
    public override void Update_KnockbackRelated()
    {
        if (Knockback_Velocity != Vector3.zero)
        {
            Knockback_Velocity *= 0.9f;
            controller = GetComponent<CharacterController>();
            controller.Move(Knockback_Velocity * Time.deltaTime);
            if (Knockback_Velocity.magnitude < 0.5f) Knockback_Velocity = Vector3.zero;
        }
    }
    public override void OnDeath()
    {
        if(IsAlive)
        {
            DeathSound.Play();
            Debug.Log("Player Is Dead");
            myAnim.SetBool("IsDead", true);
            IsAlive = false; 
        }
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
    public override void Take_Knockback(float Amount, Vector3 Direction)
    {
        Vector3 KnockbackVector = Direction.normalized * Amount;
        Knockback_Velocity += KnockbackVector;
        //Debug.Log("PlayerGetKnockback" + Amount + " " + Direction);
    }
    public virtual void Mana_Gain(float Mana_)
    {
        Mana += Mana_;
        if (Mana > Mana_max) Mana = Mana_max;
        UpdateManaBar();
    }
    public virtual bool Mana_Use(float ManaUse_)
    {
        if (!Mana_Check(ManaUse_))
        {
            return false;
        }
        else
        {
            Mana -= ManaUse_;
            UpdateManaBar();
            return true;
        }
    }
    public virtual bool Mana_Check(float ManaUse_)
    {
        if (Mana < ManaUse_) return false;
        return true;
    }
    public virtual void Mana_Regen()
    {
        if (Mana_Regen_Delay > 0)
        {
            Mana_Regen_Delay -= Time.deltaTime;
        }
        else
        {
            Mana_Gain(Mana_Regen_Amount);
        }
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
    public virtual void UpdateManaBar()
    {
        float fillAmount = Mana / Mana_max;
        if (fillAmount > 1)
        {
            fillAmount = 1;
        }
        ManaBar.GetComponent<Image>().fillAmount = fillAmount;
    }
    //public virtual void UpdateStaminaBar(){}
}
