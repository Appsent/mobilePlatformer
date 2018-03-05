using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour
{
    public Transform groundCheckPosition;

    public float groundBuffer;
    private float length;
    private float height;
    private Vector2 dimensions;
    private float topMost;

    public bool debugOn;

    void Start()
    {
        length = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        height = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;

        dimensions = new Vector2(length, height);

        topMost = transform.position.y + dimensions.y / 2;

        if (debugOn)
        {
            Debug.Log(topMost - groundBuffer);
        }
    }

    void Update()
    {
        if(topMost - groundBuffer > groundCheckPosition.position.y)
        {
            gameObject.layer = 16;
        }

        else
        {
            gameObject.layer = 13;
        }
    }

}
