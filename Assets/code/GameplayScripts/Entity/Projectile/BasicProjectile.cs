using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Reference https://www.youtube.com/watch?v=0jGL5_DFIo8
public class BasicProjectile : BasicEntity
{
    public LayerMask whatIsEnemies;
    public LayerMask whatIsPlayer;

    [Range(0.0f, 1.0f)] protected float bounciness;
    [SerializeField] protected bool useGravity;
    protected PhysicMaterial physics_mat;
    public int collisions_max = 0;
    private int collisions=0;
    public float maxLifetime = 10;
    //Targetsettings
    public GameObject ShootingFrom;
    public float Projectile_Damage = 10;
    protected Rigidbody RigidBody;
    protected override void Start()
    {
        base.Start();
        Setup();
    }
    protected override void Update()
    {
        //if (collisions < collisions_max) OnDeath();
        maxLifetime -= Time.deltaTime;
        if (maxLifetime < 0) OnDeath();
    }
    public override void OnDeath()
    {
        base.OnDeath();
    }
    public virtual void SetProjectileStatus(GameObject ShootingFrom,float Projectile_Damage = 1)
    {
        this.ShootingFrom = ShootingFrom;
        this.Projectile_Damage = Projectile_Damage;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (ShootingFrom == null) return;
        if (other.tag != ShootingFrom.tag && other.tag != "Untagged")
        {
            other.GetComponent<BasicEntity>().Take_Damage(Projectile_Damage);
            Vector3 KnockbackDirection = other.transform.position - transform.position;
            other.GetComponent<BasicEntity>().Take_Knockback(5, KnockbackDirection);
            this.OnDeath();
        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (ShootingFrom == null) return;
        if(collision.collider.CompareTag("Untagged")){ //hit with no tag
            collisions++;
            maxLifetime -= 1.0f;
            if (collisions > collisions_max) this.OnDeath();
        }
        else if(collision.collider.CompareTag(ShootingFrom.tag)) //it hit to shooting owner
        {
            collisions++;
            maxLifetime -= 1.0f;
            if (collisions > collisions_max) this.OnDeath();
        } 
        else if(!collision.collider.CompareTag(this.tag)) 
        {
            //Debug.Log(collision.collider.tag);
            collision.collider.GetComponent<BasicEntity>().Take_Damage(Projectile_Damage);
            Vector3 KnockbackDirection = collision.collider.transform.position - transform.position;
            collision.collider.GetComponent<BasicEntity>().Take_Knockback(20, KnockbackDirection);
            //Debug.Log("2");
            this.OnDeath();
        }
    }
    private protected void Setup()
    {
        RigidBody = GetComponent<Rigidbody>();
        
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;
        
        RigidBody.useGravity = useGravity;
    }
}
