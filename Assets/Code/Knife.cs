using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [SerializeField]
    private float Speed;
    private Rigidbody2D myRidgidBody;
    private Vector2 direction;

	void Start ()
    {
        myRidgidBody = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
    {
        myRidgidBody.velocity = direction * Speed;
	}

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
