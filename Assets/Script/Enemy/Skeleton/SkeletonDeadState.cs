using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SkeletonDeadState : EnemyState
{
    E_Skeleton enemy;
    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, E_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemyBase.anim.SetBool("Move",false);
        enemy.SetVelocity(0, 0);
        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);

    }

    public override void Update()
    {
        base.Update();
        enemy.fx.StartCoroutine("FlashFX");
        enemy.fx.InvokeRepeating("RedColourBlink", 0, 1f);

        Vector3 targetPosition = enemy.transform.position + new Vector3(0, 3, 0);

        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPosition, 4f * Time.deltaTime);

        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.boomState);
        }

    }
}
