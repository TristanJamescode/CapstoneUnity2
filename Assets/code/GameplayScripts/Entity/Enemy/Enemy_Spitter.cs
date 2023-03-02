using UnityEngine;
public class Enemy_Spitter : BasicEnemy
{  
    public BasicProjectile bullet;
    public float shootForce;
    [SerializeField]
    GameObject shootingPoint;
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
        transform.LookAt(Player);
        if (attack_ready)
        {
            timeToAttack = timeBetweenAttacks;
            Vector3 direction = transform.rotation * Vector3.forward;
            BasicProjectile currentBullet = Instantiate(bullet, shootingPoint.transform.position, Quaternion.identity);
            currentBullet.SetProjectileStatus(this.gameObject, 10);
            currentBullet.transform.rotation = transform.rotation;
            currentBullet.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);
            attack_ready = false;
            return true;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
            if (timeToAttack < 0) attack_ready = true;
            return false;
        }
    }
}
