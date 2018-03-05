using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    public enum BoundsBehavior
    {
        Nothing,
        Constrain,
        Kill
    }

    public BoxCollider2D Bounds;
    public BoundsBehavior Above;
    public BoundsBehavior Below;
    public BoundsBehavior Left;
    public BoundsBehavior Right;

    private Player _player;
    private BoxCollider2D _boxCollider;

    public void Start()
    {
        _player = GetComponent<Player>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (_player.IsDead)
            return;

        var colliderSize = new Vector2(
            _boxCollider.size.x * Mathf.Abs(transform.localScale.x), 
            _boxCollider.size.y * Mathf.Abs(transform.localScale.y)) / 2;
        if (Above != BoundsBehavior.Nothing && transform.position.y + colliderSize.y > Bounds.bounds.max.y)
            ApplyBoundsBehavior(Above, new Vector2(transform.position.x, Bounds.bounds.max.y - colliderSize.y));

        if (Below != BoundsBehavior.Nothing && transform.position.y - colliderSize.y < Bounds.bounds.min.y)
            ApplyBoundsBehavior(Below, new Vector2(transform.position.x, Bounds.bounds.min.y + colliderSize.y));

        if (Right != BoundsBehavior.Nothing &&transform.position.x + colliderSize.x > Bounds.bounds.max.x)
            ApplyBoundsBehavior(Right, new Vector2(Bounds.bounds.max.x - colliderSize.x, transform.position.y));

        if (Left != BoundsBehavior.Nothing && transform.position.x - colliderSize.x < Bounds.bounds.min.x)
            ApplyBoundsBehavior(Left, new Vector2(Bounds.bounds.min.x + colliderSize.x, transform.position.y));

    }

    private void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainedPosition)
    {
        if(behavior == BoundsBehavior.Kill)
        {
            LevelManager.Instance.KillPlayer();
            return;
        }

        transform.position = constrainedPosition;
    }
}
