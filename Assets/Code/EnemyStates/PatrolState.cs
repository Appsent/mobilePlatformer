using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState
{
    private float patrolTimer;
    private float patrolDuration = 10.0f;
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Debug.Log("Patroling");
        Patrol();

        enemy.Move();

        if(enemy.Target != null && enemy.InThrowRange)
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
