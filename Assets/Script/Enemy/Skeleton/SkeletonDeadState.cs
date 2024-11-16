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

    public override void Enter()//进入状态机
    {
        base.Enter();
        enemyBase.anim.SetBool("Move",false);
        enemy.SetVelocity(0, 0);
        stateTimer = 1f;

        enemyBase.rb.isKinematic = true;

    }

    public override void Exit()//退出状态机
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()//状态机更新
    {
        base.Update();
        enemy.fx.StartCoroutine("FlashFX");
        enemy.fx.InvokeRepeating("RedColourBlink", 0, 1f);//视觉特效

        Vector3 targetPosition = enemy.transform.position + new Vector3(0, 3, 0);//位置移动

        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPosition, 4f * Time.deltaTime);

        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.boomState);
        }

    }
}
