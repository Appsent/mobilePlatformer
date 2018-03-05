using UnityEngine;
using System.Collections;

public abstract class EnemyCharactController : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed = 4;
    [SerializeField]
    protected Transform knifePos;

    public int MaxHealth;
    public int Health;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected int swordDamage;

    [SerializeField]
    protected int bombDamage;

    [SerializeField]
    private BoxCollider2D swordCollider;

    public abstract bool IsDead { get; }

    public bool Attack  { get; set; }

    protected bool isFacingRight;

    public Animator anim;
    [SerializeField]
    private GameObject knifePrefab;
    public bool TakingDamage { get; set; }

    void Start ()
    {
        anim.GetComponent<Animator>();
        isFacingRight = true;
        swordCollider.enabled = false;
        Health = MaxHealth;
    }

	void Update ()
    {
      
    }

    public void ChangeDirection()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y * 1, 1);
        movementSpeed = movementSpeed * -1;
    }

     public virtual void ThrowKnife(int value)
    {
        if (isFacingRight)
        {
           GameObject tmp = (GameObject) Instantiate(knifePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,0 )));
           tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
           GameObject tmp = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
           tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public abstract IEnumerator TakeKnifeDamage();
    public abstract IEnumerator TakeSwordDamage();
    public abstract IEnumerator TakeBombDamage();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "projectile" && Health > 0)
        {
            StartCoroutine(TakeKnifeDamage());
        }

        if(other.tag == "attacktrigger" && Health > 0)
        {
            StartCoroutine(TakeSwordDamage());
        }

        if (other.tag == "bomb" && Health > 0)
        {
            StartCoroutine(TakeBombDamage());
        }
    }

    public void SwordAttack()
    {
        swordCollider.enabled = !swordCollider.enabled;
    }
}
