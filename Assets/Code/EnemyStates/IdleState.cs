using UnityEngine;
using System.Collections;

public class IdleState : IEnemyState
{
    private Enemy enemy;
    private float IdleTimer;
    private float IdleDuration = 5.0f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Debug.Log("Idling");
        Idle();

        if(enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }

    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void Idle()
    {
        enemy.anim.SetFloat("Speed", 0);

        IdleTimer += Time.deltaTime;
        if(IdleTimer >= IdleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
