using UnityEngine;

public class SimpleAttackTrigger : AttackTrigger, ITakeDamage
{
    public int Damage;
    public int PointsToGiveToPlayer;


    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGiveToPlayer != 0)
        {
            var attack = instigator.GetComponent<AttackTrigger>();
            if (attack != null && attack.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGiveToPlayer);
            }
        }
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);

    }
}
