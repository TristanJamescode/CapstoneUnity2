using UnityEngine;
public class Enemy_Contactor : BasicEnemy
{
    public float PokeDistance = 0.7f;
    public Projectile_Poke Projectile;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override bool AttackPlayer()
    {
        agent.SetDestination(transform.position); //Make Enemy does not move
        Vector3 lookAtPosition = new Vector3(Player.position.x, transform.position.y, Player.position.z);
        transform.LookAt(lookAtPosition);
        if (attack_ready)
        {
            timeToAttack = timeBetweenAttacks;
            Vector3 direction = transform.rotation * Vector3.forward;
            Projectile_Poke currentBullet = Instantiate(Projectile, transform.position, Quaternion.identity);
            currentBullet.transform.rotation = transform.rotation;
            currentBullet.PokeDistance = PokeDistance;
            currentBullet.maxLifetime = 10f;
            currentBullet.TransformUser = this.transform;
            currentBullet.PokeTime = 0.4f;
            currentBullet.Projectile_Damage = 1.0f;
            attack_finished = true;
            attack_ready = false;
            return true;
        }
        else
        {
            attack_finished = false;
            timeToAttack -= Time.deltaTime;
            if (timeToAttack < 0) attack_ready = true;
            return false;
        }
    }
}
