using UnityEngine;

public class SimpleProjectile: Projectile, ITakeDamage
{
    public int Damage;
    public GameObject DestroyedEffect;
    public int PointsToGiveToPlayer;
    public float TimeToLive;

    public void Update()
    {
        if((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }

        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if(PointsToGiveToPlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if(projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGiveToPlayer);
            }
        }

        DestroyProjectile();
    }

    protected override void OnCollideOther(Collider2D other)
    {
        DestroyProjectile();
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
