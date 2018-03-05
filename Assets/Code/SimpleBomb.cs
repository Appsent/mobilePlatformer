using UnityEngine;

public class SimpleBomb : Bomb, ITakeDamage
{
    public int Damage;
    public GameObject DestroyedEffect;
    public int PointsToGiveToPlayer;
    public float TimeToLive;

    public CircleCollider2D _explosion;

    void Awake()
    {
        _explosion.enabled = false;
    }

    public void Explode()
    {
        _explosion.enabled = true;
    }

    public void Update()
    {
        if ((TimeToLive -= Time.deltaTime) <= 0)
        {
            _explosion.enabled = false;
            DestroyBomb();
            return;
        }

        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGiveToPlayer != 0)
        {
            var bomb = instigator.GetComponent<Bomb>();
            if (bomb != null && bomb.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGiveToPlayer);
            }
        }

        DestroyBomb();
    }

    protected override void OnCollideOther(Collider2D other)
    {
        DestroyBomb();
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyBomb();
    }

    private void DestroyBomb()
    {
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
