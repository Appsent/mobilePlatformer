using UnityEngine;
using System.Collections;
using System;

public class RangedState : IEnemyState
{
    private Enemy enemy;
    private float throwTimer;
    private float coolDown = 3.0f;

    private bool canThrow = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void Execute()
    {
        Throw();
        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }

        else if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
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

    private void Throw()
    {
        throwTimer += Time.deltaTime;
        if (throwTimer >= coolDown)
        {
            canThrow = true;
            throwTimer = 0;       
        }

        else if (canThrow)
        {
            canThrow = false;
            enemy.anim.SetTrigger("Throw");
        }        
    }
}
