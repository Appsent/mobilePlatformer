using UnityEngine;
using System.Collections;
using System;

public class Enemy : EnemyCharactController, IPlayerRespawnListener
{
    private IEnemyState currentState;
    public GameObject Target { get; set; }
    private Vector2 _startPosition;
    [SerializeField]
    private EnemyHealthBar heatlthBar;

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float throwRange;

    [SerializeField]
    private int attackPoints;

    [SerializeField]
    private int bombPoints;

    [SerializeField]
    private int knifePoints;

    [SerializeField]
    private int deathPoints;

    public bool InMeleeRange

    {
        get
        {
            if(Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return Health <= 0;
        }
    }

    void Awake ()
    {
        ChangeState(new IdleState());
        _startPosition = transform.position;

    }

    void Update()
    {
        if (Health > 0)
        {
            heatlthBar.gameObject.SetActive(true);
        }
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if(Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if(xDir < 0 && isFacingRight || xDir > 0 && !isFacingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsTag("EnemyAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("EnemyThrow"))
        {
            anim.SetFloat("Speed", 1);
            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
        }
    }

    public Vector2 GetDirection()
    {
        return isFacingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeKnifeDamage()
    {
        Health -= damage;
        GameManager.Instance.AddPoints(knifePoints);
        if (!IsDead)
        {
                anim.SetTrigger("hurt");
        }

        else
        {
            anim.SetTrigger("death");
            Health = 0;
            GameManager.Instance.AddPoints(deathPoints);
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
            heatlthBar.gameObject.SetActive(false);
        }
    }

    public override IEnumerator TakeBombDamage()
    {
        Health -= bombDamage;
        GameManager.Instance.AddPoints(bombPoints);
        if (!IsDead)
        {
            anim.SetTrigger("hurt");
        }

        else
        {
            anim.SetTrigger("death");
            Health = 0;
            GameManager.Instance.AddPoints(deathPoints);
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
            heatlthBar.gameObject.SetActive(false);
        }
    }

    public override IEnumerator TakeSwordDamage()
    {
        Health -= swordDamage;
        GameManager.Instance.AddPoints(attackPoints);
        if (!IsDead)
        {
            anim.SetTrigger("hurt");
        }

        else
        {
            anim.SetTrigger("death");
            Health = 0;
            GameManager.Instance.AddPoints(deathPoints);
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
            heatlthBar.gameObject.SetActive(false);
        }        
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        Health = MaxHealth;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }

    
}
