using UnityEngine;
using System.Collections;
using System;

public class MeleeState : IEnemyState
{
    private Enemy enemy;
    private float attackTimer;
    private float coolDown = 3.0f;
    private bool canAttack = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if(enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new RangedState());
        }

        else if(enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= coolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }

        else if (canAttack)
        {
            canAttack = false;
            enemy.anim.SetTrigger("Attack");
        }
    }
}
