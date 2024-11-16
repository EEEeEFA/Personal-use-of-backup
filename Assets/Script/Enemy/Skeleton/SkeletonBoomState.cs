using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoomState : EnemyState
{
    public SkeletonBoomState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyBase.rb.isKinematic = true;
    }

    public override void Exit()//对象销毁通过  动画机调用SkeletonAnima中的BoomAndDrop()实现
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
