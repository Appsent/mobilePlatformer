using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour, ITakeDamage
{

    private LevelManagerNew lmn;
    private bool _isFacingRight;
    private CharacterController2D _controller;
    private ControllerParameters2D _parameters;
    private float _normalizedHorizontalSpeed;

    public Animator Animator;

    [SerializeField]
    private Image sneak;
    [SerializeField]
    private Image bomb;
    [SerializeField]
    private Transform sneakBar;

    public ParticleSystem sneakParticle;

    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public float FireRate;
    public float BombRate;
    private Color alpha;

    [SerializeField]
    private float attackCd;

    [SerializeField]
    private float immortalCD;
    private float immortalTime;
    public int MaxHealth = 100;
    public GameObject OuchEffect;
    public Projectile Projectile;
    public Bomb Bomb;
    public Collider2D attackTrigger;
    public Transform ProjectileFireLocation;
    public Transform BombLocation;
    public Transform groundChecker;
    private bool immortal;
    private SpriteRenderer render;
    
    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    
    private bool attacking = false;
    private float attackTimer = 0;

    private float _canFireIn;
    private float _canLayBombIn;

    public float sneakDuration = 5f;

    public float sneaking { get; private set; }

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        Health = MaxHealth;
        sneaking = sneakDuration;
        attackTrigger.enabled = false;
        render = GetComponent<SpriteRenderer>();
        immortal = false;
        _controller.gliding = false;
        alpha = GetComponent<SpriteRenderer>().color;

         if (PlayerPrefs.GetInt("Level4_points") >= 25)
         {
        sneak.gameObject.SetActive(true);
            sneakBar.localPosition = new Vector3(-386.2f, 190.1028f, 0f);
        }

        else
        {
            sneak.gameObject.SetActive(false);
            sneakBar.localPosition = new Vector3(-386.2f, 1090.1028f, 0f);
        }

        if (PlayerPrefs.GetInt("Level3_points") >= 25)
        {
            bomb.gameObject.SetActive(true);
        }

        else
        {
            bomb.gameObject.SetActive(false);
        }

    }

    public void Update()
    {
        GetComponent<SpriteRenderer>().color = alpha;
        _canFireIn -= Time.deltaTime;
        _canLayBombIn -= Time.deltaTime;
        immortalTime -= Time.deltaTime;

        if (alpha.a >= 1)
        {
            sneakParticle.Stop();
        }
        if (alpha.a <=0.5f)
        {
            sneaking -= Time.deltaTime;
            if (sneaking <= 0)
            {
                sneaking = 0;
            }
        }

        if(attackTrigger.enabled || IsDead || Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || Animator.GetCurrentAnimatorStateInfo(1).IsTag("Fire") || Animator.GetCurrentAnimatorStateInfo(0).IsTag("Crouch") || alpha.a >= 1 || sneaking <= 0)
        {
            alpha.a = 1.0f;
            GetComponent<SpriteRenderer>().color = alpha;
            gameObject.layer = 11;          
            sneaking += (Time.deltaTime/15);
            
            if(sneaking >= sneakDuration)
            {
                sneaking = sneakDuration;
            }
        }

        if (_controller.State.IsGrounded)
        {
            _controller.gliding = false;
        }

        if (_controller.gliding)
        {
            _controller.SetVerticalForce(-2.5f);
        }

        if (immortalTime <= 0)
        {
            immortal = false;
        }

        Physics2D.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("PassablePlatforms"));

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        if (IsDead || Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || Animator.GetCurrentAnimatorStateInfo(1).IsTag("Fire") || Animator.GetCurrentAnimatorStateInfo(0).IsTag("Crouch"))
            _controller.SetHorizontalForce(0);
        else
            _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));

        if (attacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
        }

        Animator.SetBool("IsGrounded", _controller.State.IsGrounded);
        Animator.SetBool("IsDead", IsDead);
        Animator.SetFloat("Speed", Mathf.Abs(_controller.Velocity.x) / MaxSpeed);

        Animator.SetBool("Gliding", _controller.gliding);
    }

    public void FinishLevel()
    {
        enabled = false;
        _controller.enabled = false;
    }

    public void Kill()
    {
        //_controller.HandleCollisions = false;
        GetComponent<Collider2D>().enabled = false;

        IsDead = true;
        Health = 0;
        //_controller.SetForce(new Vector2(0, 10));
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!_isFacingRight)
            Flip();

        IsDead = false;
        GetComponent<Collider2D>().enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth;
        sneaking = sneakDuration;

        transform.position = spawnPoint.position;
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        
        if (!immortal)
        {
            immortal = true;
            immortalTime = immortalCD;
            
            StartCoroutine(IndicateImmortal());
            Instantiate(OuchEffect, transform.position, transform.rotation);
            Health -= damage;
            Animator.SetTrigger("Hurt");
        }

        if (Health <= 0)
            LevelManager.Instance.KillPlayer();
    }

    public void GiveHealth(int health, GameObject instigator)
    {
        Health = Mathf.Min(Health + health, MaxHealth);
    }

    public void LeftDown()
    {
        if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || Animator.GetCurrentAnimatorStateInfo(1).IsTag("Fire"))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }
    }

    public void RightDown()
    {
        if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || Animator.GetCurrentAnimatorStateInfo(1).IsTag("Fire"))
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }
    }

    public void Jump()
    {
        if (_controller.CanJump)
        {
            _controller.gliding = false;
            _controller.Jump();
        }
       
    }

    public void Glide()
    {
        if (!_controller.CanJump  && PlayerPrefs.GetInt("Level2_points") >= 25)
        {
            _controller.gliding = true;
        }   
    }

    public void EndGlide()
    {
     _controller.gliding = false;
    }

    public void Idle()
    {
        _normalizedHorizontalSpeed = 0;
    }

    public void ThrowItem()
    {
        Animator.SetTrigger("Fire");
    }

    public void AttackAnim()
    { 
        
        if (!attacking && attackTimer<=0)
        {
            Animator.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        if (!attacking && attackTimer <= 0)
        {
            attacking = true;
            attackTimer = attackCd;
            attackTrigger.enabled = true;
        }
    }

    public void Crouch()
    {
        if (PlayerPrefs.GetInt("Level3_points") >= 25)
        {
            if (_canLayBombIn <= 0)
            {
                Animator.SetTrigger("Crouch");
            }
        }
    }

    public void LayBomb()
    {
        if (!attackTrigger.enabled)
        {
            if (_canLayBombIn > 0)
                return;
            
            var direction = _isFacingRight ? Vector2.right : -Vector2.right;

            var bomb = (Bomb)Instantiate(Bomb, BombLocation.position, BombLocation.rotation);
            bomb.Initialize(gameObject, direction, _controller.Velocity);
            
            _canLayBombIn = BombRate;
        }
    }

    public void FireProjectile()
    {
        if (attackTrigger.enabled == false)
        {
            if (_canFireIn > 0)
                return;

            var direction = _isFacingRight ? Vector2.right : -Vector2.right;

            var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            projectile.Initialize(gameObject, direction, _controller.Velocity);

            _canFireIn = FireRate;
        }

    }

    public void Sneak()
    {
        if (PlayerPrefs.GetInt("Level4_points") >= 25) {
            if (alpha.a <= 0.5f)
            {
                alpha.a = 1;
                gameObject.layer = 11;
                sneakParticle.Stop();
            }

            else if (!attackTrigger.enabled || !IsDead || !Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || !Animator.GetCurrentAnimatorStateInfo(1).IsTag("Fire") || !Animator.GetCurrentAnimatorStateInfo(0).IsTag("Crouch") && alpha.a >=1) {
                alpha.a = 0.5f;
                gameObject.layer = 17;
                sneakParticle.Play();
                
            }
        }     

    }

    private void Flip()
    {

        if (Time.timeScale == 1)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _isFacingRight = transform.localScale.x > 0;
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal && Health > 0)
        {
            render.enabled = false;
            yield return new WaitForSeconds(0.1f);
            render.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
