using UnityEngine;

public class SimpleEnemy : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public float Speed;
    public float FireRate = 1;
    public Projectile Projectile;
    public GameObject DestroyedEffect;
    public int attackPoints;
    public int shurikenPoints;
    private CharacterController2D _controller;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _canFireIn;

    public int MaxHealth = 100;
    public int Health { get; private set; }

    public void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);
        _startPosition = transform.position;
        Health = MaxHealth;
    }

    public void Update()
    {
        _controller.SetHorizontalForce(_direction.x * Speed);

        if((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((_canFireIn -= Time.deltaTime) > 0)
            return;

        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));

        if (!raycast)
            return;

        var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate;
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        Health -= damage;
        
        if (shurikenPoints != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            var attack = instigator.GetComponent<AttackTrigger>();

            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(shurikenPoints);
            }

            if (attack)
            {
                GameManager.Instance.AddPoints(attackPoints);
            }
        }

        if (Health <= 0) { 
        Instantiate(DestroyedEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
        }
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }
}
