using System;
using UnityEngine;

public class PathedProjectile : MonoBehaviour, ITakeDamage
{
    private Transform _destination;
    public GameObject DestroyEffect;
    private float _speed;
    public int PointsToGivePlayer;

    public void Initalize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);
        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
        if (distanceSquared > 0.01f * 0.01f)
            return;

        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);

        var projectile = instigator.GetComponent<Projectile>();
        if(projectile != null && projectile.Owner.GetComponent<Player>() != null && PointsToGivePlayer != 0)
        {
            GameManager.Instance.AddPoints(PointsToGivePlayer);
        }
    }
}
