using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Reference https://www.youtube.com/watch?v=0jGL5_DFIo8
public class BasicProjectile : BasicEntity
{
    public LayerMask whatIsEnemies;
    public LayerMask whatIsPlayer;

    [Range(0.0f, 1.0f)]
    public float bounciness;
    public bool useGravity;
    PhysicMaterial physics_mat;
    //Projectile
    public int collisions_max = 0;
    public int collisions=0;
    public float maxLifetime = 10;
    //Targetsettings
    public ProjectileFrom projectilefrom = ProjectileFrom.Player;
    public float Projectile_Damage = 10;
    public enum ProjectileFrom
    {
        Player,
        Enemy,
        Other
    }
    private void Start()
    {
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
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BasicEntity>().Take_Damage(Projectile_Damage);
            OnDeath();
        }
    }
    private protected void Setup()
    {
        /*
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;
        */
        GetComponent<Rigidbody>().useGravity = useGravity;
    }
}
